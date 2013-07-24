using Ninject.Modules;
using ReactiveUI;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;
using Theodorus2.Views;

namespace Theodorus2.Support
{
    public class Theodorus2Module : NinjectModule
    {
        public override void Load()
        {
            Bind<MainWindowView, IAboutDialogService, IConnectionInformationService>().To<MainWindowView>().InSingletonScope();
            Bind<MainWindowViewModel>().ToSelf();

            Bind<IMessageBus>().To<MessageBus>().InSingletonScope();
            Bind<IQueryExecutionService>().To<SqliteQueryExecutionService>().InSingletonScope();

            Bind<IStatusListener>().To<DefaultStatusListener>();
            Bind<IStatusReporter>().To<DefaultStatusReporter>();

            Bind<About>().ToSelf();
            Bind<ConnectionDialog, IFileSelectorService>().To<ConnectionDialog>().InSingletonScope();
            Bind<IConnectionDialogViewModel>().To<ConnectionDialogViewModel>();

            //Bind<IViewModel>().To<StatusLogViewModel>().InSingletonScope();
            //Bind<IViewModel>().To<DefaultDisplayViewModel>().InSingletonScope();
            //Bind<IViewModel>().To<DefaultRealViewModel>().InSingletonScope();
            //Bind<IViewModel>().To<DefaultRealViewModel2>().InSingletonScope();
            //Bind<IViewModel>().To<PythonInterpreterViewModel>().InSingletonScope();

            //Bind<IScriptExecutionService>().To<PythonScriptExecutionService>();
            //Bind<IStatusRepository>().To<StatusRepository>().InSingletonScope();
        }
    }
}
