using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using ActionsModule;
using Prism.Events;
using ImageModule;
using MenuModule;

namespace OpenCVPlayground
{
    public class Bootstrapper : UnityBootstrapper
    {
        EventAggregator ea;

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(ActionsModuleModule));
            catalog.AddModule(typeof(ImageModuleModule));
            catalog.AddModule(typeof(MenuModuleModule));
        }
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterInstance<IEventAggregator>(ea = new EventAggregator());
        }
    }
}
