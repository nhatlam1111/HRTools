using System.Text;
using Newtonsoft.Json;
using Serilog;
using SyncDataSqlToWebApi.Models;

namespace SyncDataSqlToWebApi.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _endpoint;
    private readonly int _maxRowPerRequest;

    public ApiService(string baseUrl, string endpoint, int maxRowPerRequest, int timeoutSeconds)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _endpoint = endpoint.TrimStart('/');
        _maxRowPerRequest = maxRowPerRequest;

        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(timeoutSeconds)
        };
    }

    public async Task<(bool success, string message)> SendBatchAsync(string procedure, List<List<object>> parameters)
    {
        try
        {
            var request = new ApiRequest
            {
                proc = procedure,
                para = parameters
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_baseUrl}/{_endpoint}";
            Log.Debug("Sending {Count} records to API: {Url}", parameters.Count, url);

            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Log.Debug("API response: {Response}", responseContent);
                return (true, responseContent);
            }
            else
            {
                Log.Warning("API returned error status {StatusCode}: {Response}", response.StatusCode, responseContent);
                return (false, $"HTTP {response.StatusCode}: {responseContent}");
            }
        }
        catch (TaskCanceledException ex)
        {
            Log.Error(ex, "API request timeout");
            return (false, "Request timeout");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error sending batch to API");
            return (false, ex.Message);
        }
    }

    public async Task<(int successCount, int failureCount)> SendDataInBatchesAsync(
        string jobName,
        string procedure, 
        List<List<object>> allParameters, 
        List<string> allHashes,
        SyncStateManager stateManager)
    {
        if (allParameters.Count != allHashes.Count)
        {
            throw new ArgumentException("Parameters and hashes count mismatch");
        }

        int successCount = 0;
        int failureCount = 0;

        // Chunk data into batches (similar to lodash.chunk in the JavaScript code)
        var totalBatches = (int)Math.Ceiling((double)allParameters.Count / _maxRowPerRequest);
        
        Log.Information("[{JobName}] Splitting {TotalRecords} records into {BatchCount} batches", 
            jobName, allParameters.Count, totalBatches);

        for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
        {
            var skip = batchIndex * _maxRowPerRequest;
            var batchParams = allParameters.Skip(skip).Take(_maxRowPerRequest).ToList();
            var batchHashes = allHashes.Skip(skip).Take(_maxRowPerRequest).ToList();

            Log.Information("[{JobName}] Sending batch {Current}/{Total} ({Count} records)", 
                jobName, batchIndex + 1, totalBatches, batchParams.Count);

            var (success, message) = await SendBatchAsync(procedure, batchParams);

            if (success)
            {
                await stateManager.MarkBatchAsSuccessAsync(jobName, batchHashes, message);
                successCount += batchParams.Count;
                Log.Information("[{JobName}] Batch {Current}/{Total} succeeded", 
                    jobName, batchIndex + 1, totalBatches);
            }
            else
            {
                await stateManager.MarkBatchAsFailedAsync(jobName, batchHashes, message);
                failureCount += batchParams.Count;
                Log.Error("[{JobName}] Batch {Current}/{Total} failed: {Error}", 
                    jobName, batchIndex + 1, totalBatches, message);
            }

            // Small delay between batches to avoid overwhelming the API
            if (batchIndex < totalBatches - 1)
            {
                await Task.Delay(100);
            }
        }

        return (successCount, failureCount);
    }
}
