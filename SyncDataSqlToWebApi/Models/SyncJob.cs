namespace SyncDataSqlToWebApi.Models;

public class SyncJob
{
    public string JobName { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public string Query { get; set; } = string.Empty;
    public string Procedure { get; set; } = string.Empty;
    public List<string> ParameterColumns { get; set; } = new();
    public List<string> UniqueKeyColumns { get; set; } = new();
    public bool SkipSyncedRecords { get; set; } = true;
    public int RevalidateAfterDays { get; set; } = 0;
    public string Description { get; set; } = string.Empty;
}
