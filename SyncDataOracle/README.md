# SyncDataO  - **UniqueColumns**: Mảng các cột dùng để xác định bản ghi trùng (thay thế cho PrimaryKey nếu không có). Ví dụ: ["DEVDT", "DEVUID", "USRID"]e

Ứng dụng console .NET 8.0 để đồng bộ dữ liệu từ một bảng Oracle sang bảng khác.

## Cấu hình

Chỉnh sửa file `appsettings.json` để cấu hình:

- **SourceDatabase**: Thông tin DB nguồn (TNS, User, Password)
- **TargetDatabase**: Thông tin DB đích
- **SyncSettings**: 
  - Tên bảng nguồn và đích
  - Số ngày đồng bộ
  - Cột khóa chính
  - **SourceQuery**: Câu SQL query lấy dữ liệu từ DB nguồn. Có thể dùng placeholder {SourceTable} sẽ được thay thế tự động. Query phải có parameter :startDate cho ngày bắt đầu.
- **Logging**: Đường dẫn log, mức log tối thiểu

Ví dụ query tùy chỉnh:
```json
"SourceQuery": "SELECT ID, NAME, CREATED_DATE FROM {SourceTable} WHERE STATUS = 'ACTIVE' AND CREATED_DATE >= :startDate"
```

## Chạy ứng dụng

```bash
dotnet run
```

Hoặc chạy exe sau khi publish:

```bash
bin\Release\net8.0\win-x64\publish\SyncDataOracle.exe
```

## Setup tự động chạy khi khởi động máy tính

### Sử dụng Task Scheduler (Khuyến nghị)

1. Mở Task Scheduler (taskschd.msc)
2. Tạo task mới: Action > Create Task
3. Tab General:
   - Name: SyncDataOracle
   - Run with highest privileges: Check
   - Configure for: Windows 10
4. Tab Triggers:
   - New > At log on > Any user
5. Tab Actions:
   - New > Start a program
   - Program/script: `C:\path\to\SyncDataOracle.exe` (đường dẫn đầy đủ đến exe)
   - Start in: `C:\path\to\publish\folder` (thư mục chứa exe và appsettings.json)
6. Tab Conditions:
   - Uncheck "Start the task only if the computer is on AC power"
7. Tab Settings:
   - Check "Run task as soon as possible after a scheduled start is missed"
   - Check "If the task fails, restart every" và set 1 minute, 3 times

### Sử dụng Startup Folder

1. Tạo shortcut đến exe
2. Copy shortcut vào `%APPDATA%\Microsoft\Windows\Start Menu\Programs\Startup`

## Lưu ý

- Đảm bảo TNS names được cấu hình đúng trong tnsnames.ora
- Ứng dụng giả sử bảng nguồn có cột CREATED_DATE để lọc theo ngày
- Schema của 2 bảng phải giống nhau cho insert đơn giản