# SyncTableOracle

A lightweight .NET 8 console tool for synchronizing rows between two Oracle databases.

## Features

- Configurable Oracle connections using full TNS descriptors
- Synchronizes a single table based on a configurable lookback window
- Skips rows already present in the destination using composite keys
- Batched inserts with transaction support
- Periodic scheduling that kicks off automatically at startup
- Structured console logging via `Microsoft.Extensions.Logging`

## Configuration

All settings are stored in `appsettings.json`:

```json
{
  "SyncSettings": {
    "LookbackDays": 2,
    "RunIntervalMinutes": 60,
    "BatchSize": 500,
    "Source": {
      "Tns": "(DESCRIPTION=...)",
      "UserId": "source_user",
      "Password": "source_password"
    },
    "Destination": {
      "Tns": "(DESCRIPTION=...)",
      "UserId": "dest_user",
      "Password": "dest_password"
    },
    "Table": {
      "Name": "THR_TERMINAL_LOGS",
      "DateColumn": "EVENT_TIME",
      "KeyColumns": [ "TERMINAL_ID", "USER_ID", "EVENT_TIME" ],
      "Columns": [ "TERMINAL_ID", "USER_ID", "USER_NAME", "CARD_ID", "EVENT_TIME", "AUTH_TYPE", "AUTH_RESULT", "CREATED_DATE" ]
    }
  }
}
```

- `LookbackDays`: how many days of history to scan each run.
- `RunIntervalMinutes`: scheduler interval.
- `BatchSize`: number of rows per transaction batch when inserting.
- `KeyColumns`: columns checked to detect duplicates in the destination.
- `Columns`: exact column order to read from source and insert into destination.

## Running

```powershell
cd d:\lam.nguyen\Projects\HRTools\SyncTableOracle
dotnet run
```

Press `Ctrl+C` to shut down gracefully. Logging is written to the console.
