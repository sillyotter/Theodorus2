using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.Support;
using Xunit;
using Theodorus2.ViewModels;

namespace UnitTests
{
    public class MainWindowViewModelTest
    {
        private readonly IStatusListener _listener = new DummyStatusListener();
        private readonly IQueryExecutionService _queryExecutor = new DummyQueryExecutor();
        private readonly IResultRenderer _resultRenderer = new DummyResultRenderer();
        private readonly ITextFileIOService _textFileIOService = new DummyTextFileIOService();
        private readonly DummyPromptingService _promptingService = new DummyPromptingService();
       
        [Fact]
        public void TestCommandInitialState()
        {
            using (var target = new MainWindowViewModel(_listener, _queryExecutor, _resultRenderer, _textFileIOService,
                _promptingService, _promptingService,
                _promptingService, _promptingService, _promptingService))
            {
                Assert.False(target.ExecuteCommand.CanExecute(null));
                Assert.False(target.SaveQueryCommand.CanExecute(null));
                Assert.False(target.SaveResultsCommand.CanExecute(null));
                Assert.False(target.OpenQueryCommand.CanExecute(null));
                Assert.True(target.AboutCommand.CanExecute(null));
                Assert.True(target.OpenDatabaseCommand.CanExecute(null));
                Assert.True(target.OptionsCommand.CanExecute(null));
                
            }
        }

        [Fact]
        public async Task TestCommandOpenDatabase()
        {
            using (var target = new MainWindowViewModel(
                _listener, _queryExecutor, _resultRenderer, _textFileIOService,
                _promptingService, _promptingService,
                _promptingService, _promptingService, _promptingService))
            {
                target.OpenDatabaseCommand.Execute(null);
                target.Document.Text = "select 1;";
                Assert.True(target.ExecuteCommand.CanExecute(null));
                Assert.True(target.SaveQueryCommand.CanExecute(null));
                Assert.False(target.SaveResultsCommand.CanExecute(null));
                Assert.True(target.OpenQueryCommand.CanExecute(null));
                Assert.True(target.AboutCommand.CanExecute(null));
                Assert.True(target.OptionsCommand.CanExecute(null));

                await target.ExecuteCommand.ExecuteAsync();
                Assert.True(target.SaveResultsCommand.CanExecute(null));

                //await target.OpenQueryCommand.ExecuteAsync();
                //Assert.Equal("ASDF", target.Document.Text);
                target.AboutCommand.Execute(null);
                Assert.True(_promptingService.AboutWasShown);
                target.OptionsCommand.Execute(null);
                Assert.True(_promptingService.OptionWasShown);
            }
        }
    }

    internal class DummyPromptingService : IAboutDialogService, IConnectionInformationService, IOptionsDialogService, IFileSelectionService, IUserPromptingService
    {
        public bool AboutWasShown;
        public void ShowAboutDialog()
        {
            AboutWasShown = true;
        }

        public string GetConnectionString()
        {
            return "Data Source=:memory:";
        }

        public bool OptionWasShown;
        public void ShowOptionsDialog()
        {
            OptionWasShown = true;
        }

        public string PromptToOpenFile(string defaultExtension, string filters)
        {
            return "test.file";
        }

        public string PromptToSaveFile(string defaultExtension, string filters)
        {
            return "test.file";
        }

        public bool PromptUserYesNo(string title, string question)
        {
            return true;
        }

        public void DisplayAlert(string messaege)
        {
        }
    }

    internal class DummyTextFileIOService : ITextFileIOService 
    {
        public void WriteFile(string fileName, string data)
        {
        }

        public string ReadFile(string fileName)
        {
            return "ASDF";
        }
    }

    internal class DummyResultRenderer : IResultRenderer 
    {
        public Task<string> RenderResults(IEnumerable<IQueryResult> input)
        {
            return Task.FromResult("ASDF");
        }
    }

    internal class DummyQueryExecutor : ReactiveObject, IQueryExecutionService 
    {
        public Task<IEnumerable<IQueryResult>> Execute(string query)
        {
            return Task.FromResult(new List<IQueryResult> {new QueryResult("ASDF", 100, new DataSet())}.AsEnumerable());
        }

        private string _connectionString;

        public string ConnectionString
        {
            get { return _connectionString; }
            set { this.RaiseAndSetIfChanged(ref _connectionString, value); }
        }
    }

    internal class DummyStatusListener  : IStatusListener 
    {
        private readonly Subject<StatusReport> _sub = new Subject<StatusReport>();
        public IObservable<StatusReport> Status { get { return _sub; }}
    }
}