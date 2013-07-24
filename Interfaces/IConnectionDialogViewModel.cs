using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Input;

namespace Theodorus2.Interfaces
{
    public interface IConnectionDialogViewModel: IDisposable 
    {
        ICommand BrowseCommand { get; }
        ICommand CancelCommand { get; }
        ICommand OkCommand { get; }
        string ConnectionString { get; set; }
        int Version { get; set; }
        SynchronizationModes SyncMode { get; set; }
        bool UseUTF16Encoding { get; set; }
        bool Pooling { get; set; }
        bool BinaryGUID { get; set; }
        string DataSource { get; set; }
        int DefaultTimeout { get; set; }
        bool FailIfMissing { get; set; }
        bool LegacyFormat { get; set; }
        bool ReadOnly { get; set; }
        string Password { get; set; }
        int PageSize { get; set; }
        bool Enlist { get; set; }
        int MaxPageCount { get; set; }
        int CacheSize { get; set; }
        SQLiteDateFormats DateTimeFormat { get; set; }
        DateTimeKind DateTimeKind { get; set; }
        string DateTimeFormatString { get; set; }
        string BaseSchemaName { get; set; }
        SQLiteJournalModeEnum JournalMode { get; set; }
        IsolationLevel DefaultIsolationLevel { get; set; }
        bool ForeignKeys { get; set; }
        SQLiteConnectionFlags Flags { get; set; }

        IObservable<bool> Results { get; }
    }
}