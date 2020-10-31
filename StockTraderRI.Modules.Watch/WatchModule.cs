//using Microsoft.Practices.Unity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using StockTraderRI.Infrastructure;
using StockTraderRI.Modules.Watch.AddWatch;
using StockTraderRI.Modules.Watch.Services;
using StockTraderRI.Modules.Watch.WatchList;
//using Unity;

namespace StockTraderRI.Modules.Watch
{
    public class WatchModule : IModule
    {
        private readonly IContainerProvider container;
        private readonly IRegionManager regionManager;

        public WatchModule(IContainerProvider container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        //public void Initialize()
        //{
        //    this.container.RegisterType<IWatchListService, WatchListService>();
        //    this.container.RegisterType<AddWatchViewModel, AddWatchViewModel>();
        //    this.container.RegisterType<WatchListViewModel, WatchListViewModel>();
        //    this.regionManager.RegisterViewWithRegion(RegionNames.MainToolBarRegion,
        //                                               () => this.container.Resolve<AddWatchView>());
        //    this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
        //                                               () => this.container.Resolve<WatchListView>());
        //}

    public void OnInitialized(Prism.Ioc.IContainerProvider containerProvider)
    {


      regionManager.RegisterViewWithRegion(RegionNames.MainToolBarRegion,
                                                 () => container.Resolve<AddWatchView>());
      regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                 () => container.Resolve<WatchListView>());

      var views = regionManager.Regions[RegionNames.MainRegion].ActiveViews;
    }

    public void RegisterTypes(Prism.Ioc.IContainerRegistry containerRegistry)
    {
      containerRegistry.Register<IWatchListService, WatchListService>();
      
      ViewModelLocationProvider.Register<WatchListView, WatchListViewModel>();
      ViewModelLocationProvider.Register<AddWatchView, AddWatchViewModel>();
      
      //containerRegistry.Register<AddWatchViewModel, AddWatchViewModel>();
      //containerRegistry.Register<WatchListViewModel, WatchListViewModel>();
      //containerRegistry.Register<AddWatchView, AddWatchView>();



    }
  }
}
