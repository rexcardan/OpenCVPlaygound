using MenuModule.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModule
{
    public class MenuModuleModule : IModule
    {
        IRegionManager _regionManager;

        public MenuModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MenuRegion", typeof(MenuView));
        }
    }
}
