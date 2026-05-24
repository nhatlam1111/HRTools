using Microsoft.Extensions.Logging;
using SyncTableOracle.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncTableOracle.Services
{
    public sealed class SyncScheduler : IAsyncDisposable
    {
        private readonly OracleDataSyncService _syncService;
        private readonly ILogger<SyncScheduler> _logger;
        private readonly TimeSpan _interval;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private PeriodicTimer? _timer;

        public SyncScheduler(OracleDataSyncService syncService, ILogger<SyncScheduler> logger, SyncSettings settings)
        {
            _syncService = syncService ?? throw new ArgumentNullException(nameof(syncService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var intervalMinutes = Math.Max(1, settings?.RunIntervalMinutes ?? 60);
            _interval = TimeSpan.FromMinutes(intervalMinutes);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Scheduler starting with interval {Interval}.", _interval);

            await RunSafelyAsync(cancellationToken).ConfigureAwait(false);

            _timer = new PeriodicTimer(_interval);

            try
            {
                while (await _timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
                {
                    await RunSafelyAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Scheduler cancellation requested.");
            }
            finally
            {
                _logger.LogInformation("Scheduler stopped.");
            }
        }

        private async Task RunSafelyAsync(CancellationToken cancellationToken)
        {
            if (!await _semaphore.WaitAsync(TimeSpan.Zero, cancellationToken).ConfigureAwait(false))
            {
                _logger.LogWarning("Previous synchronization is still running. Skipping this cycle.");
                return;
            }

            try
            {
                var inserted = await _syncService.RunOnceAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Synchronization cycle completed. {Count} new rows inserted.", inserted);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Synchronization cycle cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Synchronization cycle failed with an exception.");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async ValueTask DisposeAsync()
        {
            _timer?.Dispose();
            await _semaphore.WaitAsync().ConfigureAwait(false);
            _semaphore.Dispose();
        }
    }
}
