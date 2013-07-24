using Ninject;
using Ninject.Modules;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;
using Theodorus2.Views;

namespace Theodorus2.Support
{
    public class IoCConfiguration : NinjectModule
    {
        public override void Load()
        {
            Bind<MainWindowView>().To<MainWindowView>().InSingletonScope();
            Bind<IAboutDialogService>().ToMethod(c => c.Kernel.Get<MainWindowView>());
            Bind<IConnectionInformationService>().ToMethod(c => c.Kernel.Get<MainWindowView>());
            Bind<IFileSelectionService>().ToMethod(c => c.Kernel.Get<MainWindowView>());
            Bind<IOptionsDialogService>().ToMethod(c => c.Kernel.Get<MainWindowView>());
            Bind<IUserPromptingService>().ToMethod(c => c.Kernel.Get<MainWindowView>());

            Bind<MainWindowViewModel>().ToSelf();

            Bind<IMessageBus>().To<MessageBus>().InSingletonScope();

            Bind<IQueryExecutionService>().To<SqliteQueryExecutionService>();
            Bind<IStatusListener>().To<DefaultStatusListener>();
            Bind<IStatusReporter>().To<DefaultStatusReporter>();
            Bind<ITextFileIOService>().To<DefaultTextFileIOService>();

            Bind<About>().ToSelf();
            Bind<ConnectionDialog>().ToSelf();
            Bind<OptionsDialogViewModel>().ToSelf();
            Bind<IConnectionDialogViewModel>().To<ConnectionDialogViewModel>();           
        }
    }
}
