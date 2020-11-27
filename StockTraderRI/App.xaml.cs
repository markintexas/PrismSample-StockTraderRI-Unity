using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

using StockTraderRI.Modules.Market;
using StockTraderRI.Modules.News;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.Position;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Watch;
using StockTraderRI.Modules.Watch.AddWatch;
using StockTraderRI.Modules.Watch.WatchList;
using StockTraderRI.Views;

using System.Windows;

namespace StockTraderRI
{
  public partial class App : PrismApplication
  {
    //protected override void OnStartup(StartupEventArgs e)
    //{
    //    base.OnStartup(e);
    //    StockTraderRIBootstrapper bootstrapper = new StockTraderRIBootstrapper();
    //    bootstrapper.Run();
    //    this.ShutdownMode = ShutdownMode.OnMainWindowClose;
    //}

    protected override Window CreateShell()
    {
      return Container.Resolve<Shell>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterDialog<NewsReaderView, NewsReaderViewModel>("NewsReaderDialog");
      //containerRegistry.Register<WatchListView>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
      base.CreateModuleCatalog();

      moduleCatalog.AddModule<MarketModule>();
      moduleCatalog.AddModule<PositionModule>();
      moduleCatalog.AddModule<WatchModule>();
      moduleCatalog.AddModule<NewsModule>();
      moduleCatalog.AddModule<OrdersController>();
    }

    protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
    {
      base.ConfigureDefaultRegionBehaviors(regionBehaviors);
    }

    protected override void ConfigureViewModelLocator()
    {
      base.ConfigureViewModelLocator();

      //ViewModelLocationProvider.Register<AddWatchView, AddWatchViewModel>();
      //ViewModelLocationProvider.Register<NewsReaderView, NewsReaderViewModel>();
    }
  }
}
