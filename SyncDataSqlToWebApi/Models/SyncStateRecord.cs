namespace SyncDataSqlToWebApi.Models;

public class SyncStateRecord
{
    public int Id { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string RecordHash { get; set; } = string.Empty;
    public string RecordKeys { get; set; } = string.Empty;
    public DateTime SyncedAt { get; set; }
    public string? ApiResponse { get; set; }
    public string Status { get; set; } = string.Empty;
    public int RetryCount { get; set; }
    public string? LastError { get; set; }
}

public static class SyncStatus
{
    public const string Pending = "PENDING";
    public const string Success = "SUCCESS";
    public const string Failed = "FAILED";
}
