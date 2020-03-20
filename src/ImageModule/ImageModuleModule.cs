using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Modularity;
using Prism.Regions;
using ImageModule.Views;

namespace ImageModule
{
    public class ImageModuleModule : IModule
    {
        IRegionManager _regionManager;

        public ImageModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ImageRegion", typeof(ImageView));
        }
    }
}
