# SyncDataSqlToWebApi - Hướng dẫn sử dụng nhanh

## Các bước setup

### 1. Cấu hình SQL Server
Mở [appsettings.json](appsettings.json) và cập nhật:
```json
"SqlServer": {
  "ConnectionString": "Server=YOUR_SERVER;Database=YOUR_DB;User Id=sa;Password=***;TrustServerCertificate=True"
}
```

### 2. Cấu hình API
```json
"ApiSettings": {
  "BaseUrl": "https://your-api-server.com",
  "Endpoint": "dso/bulkinsertpro",
  "MaxRowPerRequest": 500
}
```

### 3. Cấu hình Sync Jobs
Thêm/sửa jobs trong `SyncJobs` array:
```json
{
  "JobName": "YourJobName",
  "Enabled": true,
  "Query": "SELECT col1, col2 FROM table WHERE ...",
  "Procedure": "YOUR_PROCEDURE_NAME",
  "ParameterColumns": ["col1", "col2"],
  "UniqueKeyColumns": ["col1"],
  "SkipSyncedRecords": true,
  "RevalidateAfterDays": 0
}
```

### 4. Chạy ứng dụng
```bash
dotnet build
dotnet run
```

## Các lệnh CLI hữu ích

```bash
# Xem trạng thái sync
dotnet run -- state-tool stats

# Xem chi tiết records
dotnet run -- state-tool view --job YourJobName

# Reset state để sync lại
dotnet run -- state-tool reset --job YourJobName --all

# Export state ra file
dotnet run -- state-tool export --job YourJobName --output state.csv
```

## Structure dự án
```
SyncDataSqlToWebApi/
├── Program.cs              # Entry point
├── appsettings.json        # Configuration
├── Models/                 # Data models
├── Services/               # Business logic
│   ├── SqlDataService.cs
│   ├── ApiService.cs
│   ├── SyncStateManager.cs
│   └── SyncJobExecutor.cs
└── Tools/                  # CLI tools
    └── StateManagerTool.cs
```

Xem chi tiết trong [README.md](README.md)
