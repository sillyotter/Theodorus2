using Ninject.Modules;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;
using Theodorus2.Views;
using Theodorus2.Views.Dialogs;

namespace Theodorus2.Support
{
    public class IoCConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<MainWindowView>().ToSelf().InSingletonScope();
            Bind<MainWindowViewModel>().ToSelf();
            Bind<About>().ToSelf();
            Bind<ConnectionDialog>().ToSelf();
            Bind<OptionsDialog>().ToSelf();
            Bind<OptionsDialogViewModel>().ToSelf();
            Bind<ConnectionDialogViewModel>().ToSelf();

            Bind<IMessageBus>().To<MessageBus>().InSingletonScope();

            Bind<IAboutDialogService>().To<AboutDialogService>();
            Bind<IConnectionInformationService>().To<ConnectionInformationService>();
            Bind<IFileSelectionService>().To<FileSelectionService>();
            Bind<IOptionsDialogService>().To<OptionDialogService>();
            Bind<IUserPromptingService>().To<UserPromptingService>();
            Bind<IQueryExecutionService>().To<SqliteQueryExecutionService>();
            Bind<IStatusListener>().To<DefaultStatusListener>();
            Bind<IStatusReporter>().To<DefaultStatusReporter>();
            Bind<ITextFileIOService>().To<DefaultTextFileIOService>();
            Bind<IResultRenderer>().To<DefaultHtmlRenderer>();
        }
    }
}
