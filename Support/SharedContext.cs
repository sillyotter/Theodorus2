using System;
using Ninject;
using ReactiveUI;

namespace Theodorus2.Support
{
    class SharedContext
    {
        private static readonly Lazy<SharedContext> SingletonInstance =
            new Lazy<SharedContext>(() => new SharedContext());

        public static SharedContext Instance { get { return SingletonInstance.Value; }}

        private SharedContext()
        {
            Kernel = new StandardKernel();
            //RxApp.Initialize();
            RxApp.InitializeCustomResolver((obj, typ) => Kernel.Bind(typ).ToConstant(obj));
            Kernel.Load(AppDomain.CurrentDomain.GetAssemblies());
        }

        public IKernel Kernel { get; private set; }
    }
}
