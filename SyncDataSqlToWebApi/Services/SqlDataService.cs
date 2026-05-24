using System.Data;
using Microsoft.Data.SqlClient;
using Serilog;

namespace SyncDataSqlToWebApi.Services;

public class SqlDataService : IDisposable
{
    private readonly string _connectionString;
    private SqlConnection? _connection;

    public SqlDataService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            Log.Information("SQL Server connection test successful");
            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to connect to SQL Server");
            return false;
        }
    }

    public async Task<SqlConnection> GetConnectionAsync()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection = new SqlConnection(_connectionString);
            await _connection.OpenAsync();
        }
        return _connection;
    }

    public async Task<DataTable> ExecuteQueryAsync(string query)
    {
        var dataTable = new DataTable();
        
        try
        {
            var connection = await GetConnectionAsync();
            using var command = new SqlCommand(query, connection);
            command.CommandTimeout = 300; // 5 minutes timeout

            using var adapter = new SqlDataAdapter(command);
            await Task.Run(() => adapter.Fill(dataTable));

            Log.Information("Query executed successfully. Rows returned: {Count}", dataTable.Rows.Count);
            return dataTable;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error executing query: {Query}", query);
            throw;
        }
    }

    public void Dispose()
    {
        if (_connection != null)
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
            _connection.Dispose();
            _connection = null;
        }
    }
}
