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
            Bind<MainWindowView, IAboutDialogService>().To<MainWindowView>().InSingletonScope();
            Bind<MainWindowViewModel>().To<MainWindowViewModel>().InSingletonScope();

            Bind<IMessageBus>().To<MessageBus>().InSingletonScope();

            Bind<IStatusListener>().To<DefaultStatusListener>();
            Bind<IStatusReporter>().To<DefaultStatusReporter>();

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
