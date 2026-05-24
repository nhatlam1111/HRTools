# SyncDataSqlToWebApi

Console application đồng bộ dữ liệu từ SQL Server lên Web API với khả năng tracking state và quản lý multi-jobs.

## ✨ Tính năng chính

- ✅ Đồng bộ data từ SQL Server lên Web API
- ✅ Hỗ trợ **nhiều sync jobs** chạy độc lập
- ✅ **Local state tracking** (SQLite) - tránh sync duplicate
- ✅ **Batch processing** - chia data theo chunks (mặc định 500 records/batch)
- ✅ **Scheduler** - tự động chạy theo interval
- ✅ **CLI tool** - quản lý sync state
- ✅ Logging chi tiết với Serilog
- ✅ Error handling & retry mechanism

## 📦 Requirements

- .NET 8.0 SDK
- SQL Server (source database)
- Web API endpoint (target)

## 🚀 Quick Start

### 1. Cấu hình appsettings.json

```json
{
  "SqlServer": {
    "ConnectionString": "Server=localhost;Database=YourDB;User Id=sa;Password=***;TrustServerCertificate=True"
  },
  "ApiSettings": {
    "BaseUrl": "https://your-api-server.com",
    "Endpoint": "dso/bulkinsertpro",
    "MaxRowPerRequest": 500,
    "TimeoutSeconds": 120
  },
  "SyncJobs": [
    {
      "JobName": "SyncAttendance",
      "Enabled": true,
      "Query": "SELECT USER_ID, WORK_DATE, WORK_TIME, IN_OUT FROM ATTENDANCE WHERE WORK_DATE >= DATEADD(day, -7, GETDATE())",
      "Procedure": "HR_INSERT_ATTENDANCE",
      "ParameterColumns": ["USER_ID", "WORK_DATE", "WORK_TIME", "IN_OUT"],
      "UniqueKeyColumns": ["USER_ID", "WORK_DATE", "WORK_TIME"],
      "SkipSyncedRecords": true,
      "RevalidateAfterDays": 0,
      "Description": "Đồng bộ dữ liệu chấm công"
    }
  ]
}
```

### 2. Build & Run

```bash
# Build
dotnet build

# Run sync một lần
dotnet run

# Run với scheduler (cấu hình trong appsettings.json)
# Set "EnableScheduler": true, "IntervalMinutes": 5
dotnet run
```

## 📋 Cấu hình Sync Job

Mỗi job trong `SyncJobs` array có các thuộc tính:

| Thuộc tính | Mô tả | Required |
|------------|-------|----------|
| `JobName` | Tên job (unique) | ✅ |
| `Enabled` | Bật/tắt job | ✅ |
| `Query` | SQL query lấy data | ✅ |
| `Procedure` | Tên procedure trên API | ✅ |
| `ParameterColumns` | Danh sách tên cột làm parameters | ✅ |
| `UniqueKeyColumns` | Danh sách cột để tạo unique hash | ✅ |
| `SkipSyncedRecords` | Bỏ qua records đã sync | ❌ (default: true) |
| `RevalidateAfterDays` | Sync lại sau N ngày (0 = không sync lại) | ❌ (default: 0) |
| `Description` | Mô tả job | ❌ |

### Ví dụ cấu hình nhiều jobs

```json
{
  "SyncJobs": [
    {
      "JobName": "SyncAttendance",
      "Enabled": true,
      "Query": "SELECT USER_ID, WORK_DATE, WORK_TIME FROM ATTENDANCE WHERE ...",
      "Procedure": "HR_INSERT_ATTENDANCE",
      "ParameterColumns": ["USER_ID", "WORK_DATE", "WORK_TIME"],
      "UniqueKeyColumns": ["USER_ID", "WORK_DATE", "WORK_TIME"],
      "SkipSyncedRecords": true,
      "RevalidateAfterDays": 0
    },
    {
      "JobName": "SyncEmployees",
      "Enabled": true,
      "Query": "SELECT EMP_ID, EMP_NAME, DEPT_ID FROM EMPLOYEES WHERE ...",
      "Procedure": "HR_INSERT_EMPLOYEE",
      "ParameterColumns": ["EMP_ID", "EMP_NAME", "DEPT_ID"],
      "UniqueKeyColumns": ["EMP_ID"],
      "SkipSyncedRecords": true,
      "RevalidateAfterDays": 7
    }
  ]
}
```

## 🛠️ State Management Tool

CLI tool để quản lý sync state database.

### View state records

```bash
# Xem tất cả states
dotnet run -- state-tool view

# Xem state của job cụ thể
dotnet run -- state-tool view --job SyncAttendance

# Xem theo status
dotnet run -- state-tool view --job SyncAttendance --status SUCCESS
dotnet run -- state-tool view --job SyncAttendance --status FAILED
dotnet run -- state-tool view --job SyncAttendance --status PENDING

# Giới hạn số lượng hiển thị
dotnet run -- state-tool view --job SyncAttendance --limit 50
```

### Show statistics

```bash
# Statistics tất cả jobs
dotnet run -- state-tool stats

# Statistics của job cụ thể
dotnet run -- state-tool stats --job SyncAttendance
```

### Reset state

```bash
# Reset tất cả state của job
dotnet run -- state-tool reset --job SyncAttendance --all

# Reset state theo thời gian
dotnet run -- state-tool reset --job SyncAttendance --before "2025-12-20"
dotnet run -- state-tool reset --job SyncAttendance --after "2025-12-24 10:00:00"

# Reset specific record bằng hash
dotnet run -- state-tool reset --job SyncAttendance --hash "ABC123DEF456..."
```

### Cleanup old records

```bash
# Xóa SUCCESS records cũ hơn 30 ngày
dotnet run -- state-tool cleanup --days 30
```

### Export state

```bash
# Export state ra CSV
dotnet run -- state-tool export --job SyncAttendance --output "attendance-state.csv"
```

## 🔄 Workflow

```
1. Load config và khởi tạo services
   ↓
2. Test SQL Server connection
   ↓
3. Initialize SQLite state database
   ↓
4. Với mỗi enabled job:
   ├─→ Execute SQL query
   ├─→ Filter records dựa trên sync state
   ├─→ Convert to parameter arrays
   ├─→ Mark records as PENDING
   ├─→ Chunk thành batches (500/batch)
   ├─→ Send mỗi batch lên API
   ├─→ Update state: SUCCESS hoặc FAILED
   └─→ Update job metadata
   ↓
5. Cleanup old states (nếu AutoCleanup = true)
   ↓
6. Nếu Scheduler enabled: chờ interval rồi quay lại bước 4
```

## 📊 API Request Format

Application gửi request lên API theo format:

```json
POST {BaseUrl}/{Endpoint}
Content-Type: application/json

{
  "proc": "HR_INSERT_ATTENDANCE",
  "para": [
    ["001", "2025-12-24", "08:30:00", "IN"],
    ["002", "2025-12-24", "08:35:00", "IN"],
    ["003", "2025-12-24", "08:40:00", "IN"]
  ]
}
```

Mỗi item trong `para` là một array các giá trị theo thứ tự `ParameterColumns`.

## 🔍 Logging

Logs được lưu trong thư mục `logs/`:

```
logs/
  sync-20251224.log
  sync-20251225.log
```

Format log:

```
2025-12-24 10:00:00.123 [INF] SyncDataSqlToWebApi started
2025-12-24 10:00:01.234 [INF] [SyncAttendance] Starting job: Đồng bộ dữ liệu chấm công
2025-12-24 10:00:02.345 [INF] [SyncAttendance] Query returned 1250 records
2025-12-24 10:00:02.456 [INF] [SyncAttendance] Filtered: 450 records need sync, 800 already synced
2025-12-24 10:00:02.567 [INF] [SyncAttendance] Sending batch 1/1 (450 records)
2025-12-24 10:00:03.678 [INF] [SyncAttendance] Batch 1/1 succeeded
2025-12-24 10:00:03.789 [INF] [SyncAttendance] Job completed: 450 succeeded, 0 failed, 800 skipped
```

## 🛡️ Error Handling

- **SQL Connection Error**: Log lỗi, exit application
- **Query Error**: Log lỗi, skip job
- **API Error**: Mark batch as FAILED, ghi error vào state database
- **Timeout**: Mark batch as FAILED với message "Request timeout"

## 💡 Use Cases

### UC1: Daily sync - skip duplicates

```json
{
  "JobName": "DailyAttendance",
  "SkipSyncedRecords": true,
  "RevalidateAfterDays": 0
}
```
→ Chạy hàng ngày, chỉ sync records mới

### UC2: Weekly resync

```json
{
  "JobName": "EmployeeUpdate",
  "SkipSyncedRecords": true,
  "RevalidateAfterDays": 7
}
```
→ Sync lại records cũ hơn 7 ngày (để update thông tin)

### UC3: One-time import

```json
{
  "JobName": "HistoricalImport",
  "SkipSyncedRecords": false
}
```
→ Import toàn bộ, không check state

## 🔧 Troubleshooting

### API trả SUCCESS nhưng chưa insert data thực sự

```bash
# 1. Verify bằng cách check database API
# 2. Reset state để sync lại
dotnet run -- state-tool reset --job SyncAttendance --after "2025-12-24 10:00:00"

# 3. Re-run sync
dotnet run
```

### Muốn force sync lại tất cả

```bash
# Reset toàn bộ state
dotnet run -- state-tool reset --job SyncAttendance --all

# Run sync
dotnet run
```

### Check records đang PENDING

```bash
dotnet run -- state-tool view --job SyncAttendance --status PENDING
```

## 📝 Notes

- State database (`sync-state.db`) lưu tại thư mục executable
- Hash được tạo từ `UniqueKeyColumns` bằng SHA256
- Mỗi batch tối đa 500 records (có thể thay đổi trong `MaxRowPerRequest`)
- Auto cleanup xóa SUCCESS records cũ hơn 30 ngày (configurable)

## 📄 License

Internal tool for HR data synchronization.
