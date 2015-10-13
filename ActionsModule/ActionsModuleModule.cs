using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Modularity;
using Prism.Regions;
using ActionsModule.Views;

namespace ActionsModule
{
    public class ActionsModuleModule : IModule
    {
        IRegionManager _regionManager;

        public ActionsModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ActionsRegion", typeof(ActionsView));
        }
    }
}
