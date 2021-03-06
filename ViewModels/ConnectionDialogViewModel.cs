﻿using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.Support;
using ReactiveCommand = ReactiveUI.Legacy.ReactiveCommand;

namespace Theodorus2.ViewModels
{
    public class ConnectionDialogViewModel : ValidatableReactiveObject<ConnectionDialogViewModel>
    {
        private readonly ISettingsStorageService _settings;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly SQLiteConnectionStringBuilder _builder = new SQLiteConnectionStringBuilder();
        private readonly Subject<bool> _result = new Subject<bool>();

        private static string EnsureGoodDataSource(ConnectionDialogViewModel src)
        {
            var ds = src.DataSource;
            return String.IsNullOrWhiteSpace(ds) ? "Must enter a valid path" : null;
        }

        private static string EnsurePositiveInteger(int val)
        {
            return val >= 0 ? null : "Value must be a positive non zero integer";
        }

        private static string EnsurePowerOfTwo(int val)
        {
            return (val & (val - 1)) != 0 ? "Value must be a power of two" : null;
        }

        public override void Dispose()
        {
            base.Dispose();
            _compositeDisposable.Dispose();
        }

        public ConnectionDialogViewModel(IFileSelectionService fileSelectionService, ISettingsStorageService settings)
        {
            _settings = settings;

            string defaultConStr;
            if (settings.TryGetValue("DefaultConnectionString", out defaultConStr))
            {
                _builder.ConnectionString = defaultConStr;
            }
            else
            {
                _builder.BinaryGUID = true;
                _builder.CacheSize = 4096*4;
                _builder.DateTimeFormat = SQLiteDateFormats.ISO8601;
                _builder.DateTimeFormatString = "o";
                _builder.DateTimeKind = DateTimeKind.Utc;
                _builder.DefaultIsolationLevel = IsolationLevel.ReadCommitted;
                _builder.DefaultTimeout = 30;
                _builder.FailIfMissing = false;
                _builder.UseUTF16Encoding = false;
                _builder.Flags = SQLiteConnectionFlags.Default;
                _builder.ForeignKeys = true;
                _builder.JournalMode = SQLiteJournalModeEnum.Default;
                _builder.LegacyFormat = false;
                _builder.PageSize = 4*1024;
                _builder.ReadOnly = false;
                _builder.SyncMode = SynchronizationModes.Normal;
                _builder.Version = 3;
            }

            _compositeDisposable.Add(
                Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    handler => PropertyChanged += handler, handler => PropertyChanged -= handler)
                    .Where(e => e.EventArgs.PropertyName != "ConnectionString" && e.EventArgs.PropertyName != "HasErrors")
                    .Subscribe(x => this.RaisePropertyChanged(i => i.ConnectionString)));

            AddValidator(x => x.DataSource, () => EnsureGoodDataSource(this));
            AddValidator(x => x.CacheSize, () => EnsurePositiveInteger(CacheSize));
            AddValidator(x => x.DefaultTimeout, () => EnsurePositiveInteger(DefaultTimeout));
            AddValidator(x => x.MaxPageCount, () => EnsurePositiveInteger(MaxPageCount));
            AddValidator(x => x.PageSize, () => EnsurePositiveInteger(PageSize) ?? EnsurePowerOfTwo(PageSize));

            var browseCommand = new ReactiveCommand();
            _compositeDisposable.Add(
                browseCommand.Subscribe(x =>
                {
                    DataSource = fileSelectionService.PromptToOpenFile(".sqlite",
                        "Database Files (*.sqlite)|*.sqlite|Database Files (*.db)|*.db|All Files (*.*)|*.*");
                }));
            BrowseCommand = browseCommand;

            var okCommand = new ReactiveCommand(this.WhenAny(x => x.HasErrors, x => !x.GetValue()));
            _compositeDisposable.Add(okCommand);
            OkCommand = okCommand;

            var cancelCommand = new ReactiveCommand();
            _compositeDisposable.Add(cancelCommand);
            CancelCommand = cancelCommand;

            _compositeDisposable.Add(
                okCommand.Select(x => true).Merge(cancelCommand.Select(x => false)).Subscribe(x =>
                {
                    _result.OnNext(x);
                    _result.OnCompleted();
                }));

            var sdc = new ReactiveCommand();
            _compositeDisposable.Add(
            sdc.Subscribe(x =>
            {
                var nb = new SQLiteConnectionStringBuilder(_builder.ConnectionString) {DataSource = null};
                _settings.SetValue("DefaultConnectionString", nb.ConnectionString);
            }));
            SetDefault = sdc;
            _compositeDisposable.Add(sdc);

        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand OkCommand { get; private set; }
        public ICommand SetDefault { get; private set; }

        public IObservable<bool> Results => _result;

        public string ConnectionString
        {
            get { return _builder.ConnectionString; }
            set
            {
                _builder.ConnectionString = value;
                this.RaisePropertyChanged(x => x.ConnectionString);
            }
        }

        public int Version
        {
            get { return _builder.Version; }
            set
            {
                _builder.Version = value;
                this.RaisePropertyChanged(x => x.Version);
            }
        }

        public SynchronizationModes SyncMode
        {
            get { return _builder.SyncMode; }
            set
            {
                _builder.SyncMode = value;
                this.RaisePropertyChanged(x => x.SyncMode);
            }
        }

        public bool UseUtf16Encoding
        {
            get { return _builder.UseUTF16Encoding; }
            set
            {
                _builder.UseUTF16Encoding = value;
                this.RaisePropertyChanged(x => x.UseUtf16Encoding);
            }
        }

        public bool Pooling
        {
            get { return _builder.Pooling; }
            set
            {
                _builder.Pooling = value;
                this.RaisePropertyChanged(x => x.Pooling);
            }
        }

        public bool BinaryGuid
        {
            get { return _builder.BinaryGUID; }
            set
            {
                _builder.BinaryGUID = value;
                this.RaisePropertyChanged(x => x.BinaryGuid);
            }
        }

        public string DataSource
        {
            get { return _builder.DataSource; }
            set
            {
                _builder.DataSource = value;
                this.RaisePropertyChanged(x => x.DataSource);
            }
        }

        public int DefaultTimeout
        {
            get { return _builder.DefaultTimeout; }
            set
            {
                _builder.DefaultTimeout = value;
                this.RaisePropertyChanged(x => x.DefaultTimeout);
            }
        }

        public bool FailIfMissing
        {
            get { return _builder.FailIfMissing; }
            set
            {
                _builder.FailIfMissing = value;
                this.RaisePropertyChanged(x => x.FailIfMissing);
            }
        }

        public bool LegacyFormat
        {
            get { return _builder.LegacyFormat; }
            set
            {
                _builder.LegacyFormat = value;
                this.RaisePropertyChanged(x => x.LegacyFormat);
            }
        }

        public bool ReadOnly
        {
            get { return _builder.ReadOnly; }
            set
            {
                _builder.ReadOnly = value;
                this.RaisePropertyChanged(x => x.ReadOnly);
            }
        }

        public string Password
        {
            get { return _builder.Password; }
            set
            {
                _builder.Password = value;
                this.RaisePropertyChanged(x => x.Password);
            }
        }

        public int PageSize
        {
            get { return _builder.PageSize; }
            set
            {
                _builder.PageSize = value;
                this.RaisePropertyChanged(x => x.PageSize);
            }
        }

        public bool Enlist
        {
            get { return _builder.Enlist; }
            set
            {
                _builder.Enlist = value;
                this.RaisePropertyChanged(x => x.Enlist);
            }
        }

        public int MaxPageCount
        {
            get { return _builder.MaxPageCount; }
            set
            {
                _builder.MaxPageCount = value;
                this.RaisePropertyChanged(x => x.MaxPageCount);
            }
        }

        public int CacheSize
        {
            get { return _builder.CacheSize; }
            set
            {
                _builder.CacheSize = value;
                this.RaisePropertyChanged(x => x.CacheSize);
            }
        }

        public SQLiteDateFormats DateTimeFormat
        {
            get { return _builder.DateTimeFormat; }
            set
            {
                _builder.DateTimeFormat = value;
                this.RaisePropertyChanged(x => x.DateTimeFormat);
            }
        }

        public DateTimeKind DateTimeKind
        {
            get { return _builder.DateTimeKind; }
            set
            {
                _builder.DateTimeKind = value;
                this.RaisePropertyChanged(x => x.DateTimeKind);
            }
        }

        public string DateTimeFormatString
        {
            get { return _builder.DateTimeFormatString; }
            set
            {
                _builder.DateTimeFormatString = value;
                this.RaisePropertyChanged(x => x.DateTimeFormatString);
            }
        }

        public string BaseSchemaName
        {
            get { return _builder.BaseSchemaName; }
            set
            {
                _builder.BaseSchemaName = value;
                this.RaisePropertyChanged(x => x.BaseSchemaName);
            }
        }

        public SQLiteJournalModeEnum JournalMode
        {
            get { return _builder.JournalMode; }
            set
            {
                _builder.JournalMode = value;
                this.RaisePropertyChanged(x => x.JournalMode);
            }
        }

        public IsolationLevel DefaultIsolationLevel
        {
            get { return _builder.DefaultIsolationLevel; }
            set
            {
                _builder.DefaultIsolationLevel = value;
                this.RaisePropertyChanged(x => x.DefaultIsolationLevel);
            }
        }

        public bool ForeignKeys
        {
            get { return _builder.ForeignKeys; }
            set
            {
                _builder.ForeignKeys = value;
                this.RaisePropertyChanged(x => x.ForeignKeys);
            }
        }

        public SQLiteConnectionFlags Flags
        {
            get { return _builder.Flags; }
            set
            {
                _builder.Flags = value;
                this.RaisePropertyChanged(x => x.Flags);
            }
        }
    }
}
