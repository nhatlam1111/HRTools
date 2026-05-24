using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using SyncTableOracle.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncTableOracle.Services
{
    public sealed class OracleDataSyncService
    {
        private readonly SyncSettings _settings;
        private readonly ILogger<OracleDataSyncService> _logger;

        public OracleDataSyncService(SyncSettings settings, ILogger<OracleDataSyncService> logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> RunOnceAsync(CancellationToken cancellationToken)
        {
            ValidateSettings();

            var (fromDate, toDate) = CalculateDateRange();
            _logger.LogInformation("Starting synchronization for table {Table} between {From:yyyy-MM-dd HH:mm:ss} and {To:yyyy-MM-dd HH:mm:ss}.",
                _settings.Table.Name,
                fromDate,
                toDate);

            await using var sourceConnection = CreateConnection(_settings.Source);
            await using var destinationConnection = CreateConnection(_settings.Destination);

            await sourceConnection.OpenAsync(cancellationToken);
            await destinationConnection.OpenAsync(cancellationToken);

            var sourceRows = await ReadSourceRowsAsync(sourceConnection, fromDate, toDate, cancellationToken);
            _logger.LogInformation("Fetched {Count} rows from source database.", sourceRows.Count);

            if (sourceRows.Count == 0)
            {
                return 0;
            }

            var existingKeys = await ReadExistingKeysAsync(destinationConnection, fromDate, toDate, cancellationToken);
            _logger.LogInformation("Loaded {Count} existing keys from destination database.", existingKeys.Count);

            var newRows = FilterNewRows(sourceRows, existingKeys);
            _logger.LogInformation("Identified {Count} new rows to insert into destination database.", newRows.Count);

            if (newRows.Count == 0)
            {
                return 0;
            }

            var inserted = await InsertRowsAsync(destinationConnection, newRows, cancellationToken);
            _logger.LogInformation("Inserted {Count} rows into destination table {Table}.", inserted, _settings.Table.Name);

            return inserted;
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(_settings.Table.Name))
            {
                throw new InvalidOperationException("Table name must be provided in configuration.");
            }

            if (string.IsNullOrWhiteSpace(_settings.Table.DateColumn))
            {
                throw new InvalidOperationException("DateColumn must be provided in configuration.");
            }

            if (_settings.Table.Columns == null || _settings.Table.Columns.Count == 0)
            {
                throw new InvalidOperationException("At least one column must be configured for synchronization.");
            }

            if (_settings.Table.KeyColumns == null || _settings.Table.KeyColumns.Count == 0)
            {
                throw new InvalidOperationException("At least one key column must be configured to detect duplicates.");
            }
        }

        private (DateTime from, DateTime to) CalculateDateRange()
        {
            var toDate = DateTime.Now;
            var lookbackDays = Math.Max(1, _settings.LookbackDays);
            var fromDate = toDate.AddDays(-Math.Abs(lookbackDays));
            return (fromDate, toDate);
        }

        private OracleConnection CreateConnection(DbConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new InvalidOperationException("Database connection settings are missing.");
            }

            if (string.IsNullOrWhiteSpace(settings.Tns))
            {
                throw new InvalidOperationException("TNS descriptor must be provided for Oracle connections.");
            }

            if (string.IsNullOrWhiteSpace(settings.UserId))
            {
                throw new InvalidOperationException("UserId must be provided for Oracle connections.");
            }

            if (string.IsNullOrWhiteSpace(settings.Password))
            {
                throw new InvalidOperationException("Password must be provided for Oracle connections.");
            }

            var builder = new OracleConnectionStringBuilder
            {
                DataSource = settings.Tns,
                UserID = settings.UserId,
                Password = settings.Password,
            };

            builder["Pooling"] = true;
            builder["Validate Connection"] = true;

            return new OracleConnection(builder.ConnectionString);
        }

        private async Task<List<Dictionary<string, object?>>> ReadSourceRowsAsync(OracleConnection connection, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            var columns = _settings.Table.Columns;
            var columnList = string.Join(", ", columns);
            var sql = $"SELECT {columnList} FROM {_settings.Table.Name} WHERE {_settings.Table.DateColumn} BETWEEN :p_from AND :p_to ORDER BY {_settings.Table.DateColumn}";

            await using var command = new OracleCommand(sql, connection)
            {
                BindByName = true
            };

            command.Parameters.Add(CreateDateParameter(":p_from", fromDate));
            command.Parameters.Add(CreateDateParameter(":p_to", toDate));

            var rows = new List<Dictionary<string, object?>>();

            await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken).ConfigureAwait(false);
            var ordinals = columns.ToDictionary(column => column, column => reader.GetOrdinal(column), StringComparer.OrdinalIgnoreCase);

            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                foreach (var column in columns)
                {
                    var ordinal = ordinals[column];
                    var value = await reader.IsDBNullAsync(ordinal, cancellationToken).ConfigureAwait(false)
                        ? null
                        : reader.GetValue(ordinal);
                    row[column] = value;
                }

                rows.Add(row);
            }

            return rows;
        }

        private async Task<HashSet<string>> ReadExistingKeysAsync(OracleConnection connection, DateTime fromDate, DateTime toDate, CancellationToken cancellationToken)
        {
            var keyColumns = _settings.Table.KeyColumns;
            var columnList = string.Join(", ", keyColumns);
            var sql = $"SELECT {columnList} FROM {_settings.Table.Name} WHERE {_settings.Table.DateColumn} BETWEEN :p_from AND :p_to";

            await using var command = new OracleCommand(sql, connection)
            {
                BindByName = true
            };

            command.Parameters.Add(CreateDateParameter(":p_from", fromDate));
            command.Parameters.Add(CreateDateParameter(":p_to", toDate));

            var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken).ConfigureAwait(false);
            var ordinals = keyColumns.ToDictionary(column => column, column => reader.GetOrdinal(column), StringComparer.OrdinalIgnoreCase);

            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var keyParts = new List<string>(keyColumns.Count);
                foreach (var column in keyColumns)
                {
                    var ordinal = ordinals[column];
                    if (await reader.IsDBNullAsync(ordinal, cancellationToken).ConfigureAwait(false))
                    {
                        keyParts.Add(string.Empty);
                    }
                    else
                    {
                        var value = reader.GetValue(ordinal);
                        keyParts.Add(FormatKeyPart(value));
                    }
                }

                keys.Add(string.Join("|", keyParts));
            }

            return keys;
        }

        private List<Dictionary<string, object?>> FilterNewRows(IEnumerable<Dictionary<string, object?>> sourceRows, HashSet<string> existingKeys)
        {
            var keyColumns = _settings.Table.KeyColumns;
            var result = new List<Dictionary<string, object?>>();

            foreach (var row in sourceRows)
            {
                var key = BuildKey(row, keyColumns);
                if (!existingKeys.Contains(key))
                {
                    result.Add(row);
                }
            }

            return result;
        }

        private async Task<int> InsertRowsAsync(OracleConnection connection, List<Dictionary<string, object?>> rows, CancellationToken cancellationToken)
        {
            var columns = _settings.Table.Columns;
            var columnList = string.Join(", ", columns);
            var parameterList = string.Join(", ", columns.Select((c, index) => $":p{index}"));
            var sql = $"INSERT INTO {_settings.Table.Name} ({columnList}) VALUES ({parameterList})";
            var batchSize = Math.Max(1, _settings.BatchSize);
            var inserted = 0;

            using var transaction = connection.BeginTransaction();

            try
            {
                for (var i = 0; i < rows.Count; i += batchSize)
                {
                    var batch = rows.Skip(i).Take(batchSize);
                    foreach (var row in batch)
                    {
                        using var command = new OracleCommand(sql, connection)
                        {
                            BindByName = true,
                            Transaction = transaction
                        };

                        for (int c = 0; c < columns.Count; c++)
                        {
                            var column = columns[c];
                            var parameterName = $":p{c}";
                            row.TryGetValue(column, out var value);
                            command.Parameters.Add(CreateParameter(parameterName, value));
                        }

                        inserted += await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                    }
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert rows into destination table {Table}. Transaction will be rolled back.", _settings.Table.Name);
                transaction.Rollback();
                throw;
            }

            return inserted;
        }

        private static string BuildKey(Dictionary<string, object?> row, IReadOnlyCollection<string> keyColumns)
        {
            var parts = new List<string>(keyColumns.Count);
            foreach (var column in keyColumns)
            {
                _ = row.TryGetValue(column, out var value);
                parts.Add(FormatKeyPart(value));
            }

            return string.Join("|", parts);
        }

        private static string FormatKeyPart(object? value)
        {
            return value switch
            {
                null => string.Empty,
                DateTime dateTime => dateTime.ToString("O"),
                OracleTimeStamp timestamp => timestamp.Value.ToString("O"),
                OracleDate oracleDate => oracleDate.Value.ToString("O"),
                _ => Convert.ToString(value) ?? string.Empty
            };
        }

        private static OracleParameter CreateParameter(string name, object? value)
        {
            var parameter = new OracleParameter
            {
                ParameterName = name,
                Value = value ?? DBNull.Value
            };

            if (value is DateTime)
            {
                parameter.OracleDbType = OracleDbType.TimeStamp;
            }
            else if (value is OracleTimeStamp)
            {
                parameter.OracleDbType = OracleDbType.TimeStamp;
            }
            else if (value is int)
            {
                parameter.OracleDbType = OracleDbType.Int32;
            }
            else if (value is long)
            {
                parameter.OracleDbType = OracleDbType.Int64;
            }
            else if (value is decimal)
            {
                parameter.OracleDbType = OracleDbType.Decimal;
            }
            else if (value is double)
            {
                parameter.OracleDbType = OracleDbType.Double;
            }
            else if (value is float)
            {
                parameter.OracleDbType = OracleDbType.Single;
            }
            else if (value is short)
            {
                parameter.OracleDbType = OracleDbType.Int16;
            }
            else if (value is byte)
            {
                parameter.OracleDbType = OracleDbType.Byte;
            }
            else if (value is bool boolValue)
            {
                parameter.OracleDbType = OracleDbType.Int16;
                parameter.Value = boolValue ? 1 : 0;
            }
            else if (value is Guid guidValue)
            {
                parameter.OracleDbType = OracleDbType.Raw;
                parameter.Value = guidValue.ToByteArray();
            }

            return parameter;
        }

        private static OracleParameter CreateDateParameter(string name, DateTime value)
        {
            return new OracleParameter
            {
                ParameterName = name,
                OracleDbType = OracleDbType.TimeStamp,
                Value = value
            };
        }
    }
}
