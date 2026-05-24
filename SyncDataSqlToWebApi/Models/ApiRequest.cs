namespace SyncDataSqlToWebApi.Models;

public class ApiRequest
{
    public string proc { get; set; } = string.Empty;
    public List<List<object>> para { get; set; } = new();
}
