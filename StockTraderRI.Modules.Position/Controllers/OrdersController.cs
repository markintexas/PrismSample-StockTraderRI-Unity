using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Prism.Commands;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;

using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Orders;
using StockTraderRI.Modules.Position.Properties;
using StockTraderRI.Modules.Position.Services;

namespace StockTraderRI.Modules.Position.Controllers
{
  public class OrdersController : IOrdersController, IModule
  {
    private readonly IRegionManager _regionManager;
    private readonly StockTraderRICommandProxy _commandProxy;
    private readonly IAccountPositionService _accountPositionService;
    private IContainerProvider _containerProvider;

    public OrdersController(IContainerProvider containerProvider, IRegionManager regionManager, StockTraderRICommandProxy commandProxy, IAccountPositionService accountPositionService)
    {
      _regionManager = regionManager;
      _containerProvider = containerProvider;
      _accountPositionService = accountPositionService;
      _commandProxy = commandProxy ?? throw new ArgumentNullException("commandProxy");
      
      BuyCommand = new DelegateCommand<string>(OnBuyExecuted);
      SellCommand = new DelegateCommand<string>(OnSellExecuted);
      SubmitAllVoteOnlyCommand = new DelegateCommand(() => { }, SubmitAllCanExecute);
      
      OrderModels = new List<IOrderCompositeViewModel>();
      //_commandProxy.SubmitAllOrdersCommand.RegisterCommand(SubmitAllVoteOnlyCommand);
    }

    void OnSellExecuted(string parameter)
    {
      StartOrder(parameter, TransactionType.Sell);
    }

    void OnBuyExecuted(string parameter)
    {
      StartOrder(parameter, TransactionType.Buy);
    }

    virtual protected bool SubmitAllCanExecute()
    {
      //return true;

      Dictionary<string, long> sellOrderShares = new Dictionary<string, long>();

      if (OrderModels.Count == 0)
      {
        return false;
      }

      foreach (var order in OrderModels)
      {
        if (order.TransactionInfo.TransactionType == TransactionType.Sell)
        {
          string tickerSymbol = order.TransactionInfo.TickerSymbol.ToUpper(CultureInfo.CurrentCulture);
          if (!sellOrderShares.ContainsKey(tickerSymbol))
            sellOrderShares.Add(tickerSymbol, 0);

          //populate dictionary with total shares bought or sold by tickersymbol
          sellOrderShares[tickerSymbol] += order.Shares;
        }
      }

      IList<AccountPosition> positions = _accountPositionService.GetAccountPositions();

      foreach (string key in sellOrderShares.Keys)
      {
        AccountPosition position =
            positions.FirstOrDefault(
                x => String.Compare(x.TickerSymbol, key, StringComparison.CurrentCultureIgnoreCase) == 0);
        
        if (position == null || position.Shares < sellOrderShares[key])
        {
          //trying to sell more shares than we own
          return false;
        }
      }

      return true;
    }

    virtual protected void StartOrder(string tickerSymbol, TransactionType transactionType)
    {
      if (string.IsNullOrEmpty(tickerSymbol))
      {
        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "tickerSymbol"));
      }
      ShowOrdersView();

      IRegion ordersRegion = _regionManager.Regions[RegionNames.OrdersRegion];

      var orderCompositeViewModel = _containerProvider.Resolve<IOrderCompositeViewModel>();
      ordersRegion.Add(orderCompositeViewModel);
      OrderModels.Add(orderCompositeViewModel);

      orderCompositeViewModel.TransactionInfo = new TransactionInfo(tickerSymbol, transactionType);
      orderCompositeViewModel.CloseViewRequested += delegate
      {
        OrderModels.Remove(orderCompositeViewModel);
        _commandProxy.SubmitAllOrdersCommand.UnregisterCommand(orderCompositeViewModel.SubmitCommand);
        _commandProxy.CancelAllOrdersCommand.UnregisterCommand(orderCompositeViewModel.CancelCommand);
        _commandProxy.SubmitOrderCommand.UnregisterCommand(orderCompositeViewModel.SubmitCommand);
        _commandProxy.CancelOrderCommand.UnregisterCommand(orderCompositeViewModel.CancelCommand);
        ordersRegion.Remove(orderCompositeViewModel);
        if (ordersRegion.Views.Count() == 0)
        {
          RemoveOrdersView();
        }
      };


      _commandProxy.SubmitAllOrdersCommand.RegisterCommand(orderCompositeViewModel.SubmitCommand);
      _commandProxy.CancelAllOrdersCommand.RegisterCommand(orderCompositeViewModel.CancelCommand);
      _commandProxy.SubmitOrderCommand.RegisterCommand(orderCompositeViewModel.SubmitCommand);
      _commandProxy.CancelOrderCommand.RegisterCommand(orderCompositeViewModel.CancelCommand);

      ordersRegion.Activate(orderCompositeViewModel);
    }

    private void RemoveOrdersView()
    {
      IRegion region = _regionManager.Regions[RegionNames.ActionRegion];

      object ordersView = region.GetView("OrdersView");
      if (ordersView != null)
      {
        region.Remove(ordersView);
      }
    }

    private void ShowOrdersView()
    {
      IRegion region = _regionManager.Regions[RegionNames.ActionRegion];

      object ordersView = region.GetView("OrdersView");
      if (ordersView == null)
      {
        ordersView = _containerProvider.Resolve<IOrdersView>();
        region.Add(ordersView, "OrdersView");
      }

      region.Activate(ordersView);
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.Register<IOrdersView, OrdersView>();
      containerRegistry.Register<IOrderDetailsViewModel, OrderDetailsViewModel>();
      containerRegistry.Register<IOrderCompositeViewModel, OrderCompositeViewModel>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
      _containerProvider = containerProvider;
    }

    #region IOrdersController Members

    public DelegateCommand<string> BuyCommand { get; private set; }
    public DelegateCommand<string> SellCommand { get; private set; }
    public DelegateCommand SubmitAllVoteOnlyCommand { get; private set; }

    private List<IOrderCompositeViewModel> OrderModels { get; set; }

    #endregion
  }
}
