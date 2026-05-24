using System.Collections.Generic;

namespace SyncTableOracle.Configuration
{
    public sealed class SyncSettings
    {
        public int LookbackDays { get; set; } = 1;
        public int RunIntervalMinutes { get; set; } = 60;
        public int BatchSize { get; set; } = 500;
        public DbConnectionSettings Source { get; set; } = new();
        public DbConnectionSettings Destination { get; set; } = new();
        public TableSettings Table { get; set; } = new();
    }

    public sealed class DbConnectionSettings
    {
        public string Tns { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public sealed class TableSettings
    {
        public string Name { get; set; } = string.Empty;
        public string DateColumn { get; set; } = string.Empty;
        public List<string> KeyColumns { get; set; } = new();
        public List<string> Columns { get; set; } = new();
    }
}
