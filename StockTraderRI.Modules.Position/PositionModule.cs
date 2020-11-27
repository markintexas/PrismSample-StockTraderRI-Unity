//using Microsoft.Practices.Unity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Controllers;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.PositionSummary;
using StockTraderRI.Modules.Position.Services;
//using Unity;

namespace StockTraderRI.Modules.Position
{
    public class PositionModule : IModule
    {
        private readonly IContainerProvider container;
        private readonly IRegionManager regionManager;
        private OrdersController _ordersController;

        public PositionModule(IContainerProvider container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

#if MAODEBUG
    public void Initialize()
        {
            //this.container.RegisterType<IAccountPositionService, AccountPositionService>();
            //this.container.RegisterType<IOrdersService, XmlOrdersService>();
            //this.container.RegisterType<IOrdersController, OrdersController>();
            //this.container.RegisterType<IObservablePosition, ObservablePosition>();
            //this.container.RegisterType<IPositionSummaryViewModel, PositionSummaryViewModel>();
            //this.container.RegisterType<IPositionPieChartViewModel, PositionPieChartViewModel>();
            this.regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                       () => this.container.Resolve<PositionSummaryView>());
            this._ordersController = this.container.Resolve<OrdersController>();
        }
#endif
    public void OnInitialized(IContainerProvider containerProvider)
    {
      regionManager.RegisterViewWithRegion(RegionNames.MainRegion,
                                                 () => container.Resolve<PositionSummaryView>());
      regionManager.RegisterViewWithRegion(RegionNames.ResearchRegion,
                                                 () => container.Resolve<PositionPieChartView>());
      _ordersController = container.Resolve<OrdersController>();
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.Register<IAccountPositionService, AccountPositionService>();
      containerRegistry.Register<IOrdersService, XmlOrdersService>();
      containerRegistry.Register<IOrdersController, OrdersController>();
      containerRegistry.Register<IObservablePosition, ObservablePosition>();
      containerRegistry.Register<IPositionSummaryViewModel, PositionSummaryViewModel>();
      containerRegistry.Register<IPositionPieChartViewModel, PositionPieChartViewModel>();

      ViewModelLocationProvider.Register<PositionPieChartView, IPositionPieChartViewModel>();
      ViewModelLocationProvider.Register<PositionSummaryView, IPositionSummaryViewModel>();
      ViewModelLocationProvider.Register<OrdersView, OrdersViewModel>();
    }
  }
}
