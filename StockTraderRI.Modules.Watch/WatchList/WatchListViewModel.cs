using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using StockTraderRI.Modules.Watch.Services;
using StockTraderRI.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using Prism.Regions;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Watch.Properties;
using StockTraderRI.Infrastructure.Messaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace StockTraderRI.Modules.Watch.WatchList
{
  public class WatchListViewModel : BindableBase
  {
    private readonly IMarketFeedService marketFeedService;
    //private readonly IWatchListService watchListService;
    private readonly IEventAggregator eventAggregator;
    private readonly IRegionManager regionManager;
   // private readonly ObservableCollection<string> watchList;
    private readonly ICommand removeWatchCommand;
    private ObservableCollection<WatchItem> watchListItems;
    private WatchItem currentWatchItem;

    public WatchListViewModel(/*IWatchListService watchListService, */IMarketFeedService marketFeedService, IRegionManager regionManager, IEventAggregator eventAggregator)
    {
      HeaderInfo = Resources.WatchListTitle;
      WatchListItems = new ObservableCollection<WatchItem>();

      this.marketFeedService = marketFeedService;
      //this.watchListService = watchListService ?? throw new ArgumentNullException("watchListService");
      this.regionManager = regionManager;

      //watchList = watchListService.RetrieveWatchList();
      //watchList.CollectionChanged += PopulateWatchItemsList(watchList);
      //watchList.CollectionChanged += WatchList_CollectionChanged;
      //PopulateWatchItemsList(watchList);
      //WatchListItems.Add(new WatchItem("STOCK0", 1.00m));

      this.eventAggregator = eventAggregator ?? throw new ArgumentNullException("eventAggregator");
      this.eventAggregator.GetEvent<MarketPricesUpdatedEvent>().Subscribe(MarketPricesUpdated, ThreadOption.UIThread);

      this.eventAggregator.GetEvent<AddWatchStockEvent>().Subscribe(AddWatchStock, ThreadOption.UIThread);

      removeWatchCommand = new DelegateCommand<string>(RemoveWatch);

      watchListItems.CollectionChanged += WatchListItems_CollectionChanged;
    }

    private void AddWatchStock(string stockSymbol)
    {
      if( !WatchListItems.Any( x => x.TickerSymbol.Equals(stockSymbol)))
      {
        PopulateWatchItemsList(new string[] { stockSymbol });
      }
    }

    //private void WatchList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //{
    //  PopulateWatchItemsList(watchListService.RetrieveWatchList() );
    //}

    public ObservableCollection<WatchItem> WatchListItems
    {
      get
      {
        return watchListItems;
      }

      private set
      {
        SetProperty(ref watchListItems, value);
      }
    }

    public WatchItem CurrentWatchItem
    {
      get
      {
        return currentWatchItem;
      }

      set
      {
        if (value != null)
        {
          SetProperty(ref currentWatchItem, value);
          eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Publish(currentWatchItem.TickerSymbol);
        }
      }
    }

    public string HeaderInfo { get; set; }

    public ICommand RemoveWatchCommand { get { return removeWatchCommand; } }

    private void MarketPricesUpdated(IDictionary<string, decimal> updatedPrices)
    {
      if (updatedPrices == null)
      {
        throw new ArgumentNullException("updatedPrices");
      }

      foreach (WatchItem watchItem in WatchListItems)
      {
        if (updatedPrices.ContainsKey(watchItem.TickerSymbol))
        {
          watchItem.CurrentPrice = updatedPrices[watchItem.TickerSymbol];
        }
      }
    }

    private void RemoveWatch(string tickerSymbol)
    {
      var item = WatchListItems.FirstOrDefault(w => w.TickerSymbol.Equals(tickerSymbol, StringComparison.InvariantCultureIgnoreCase));
      
      if( item != null )
      {
        WatchListItems.Remove(item);
      }
    }

    private void PopulateWatchItemsList(IEnumerable<string> watchItemsList)
    {
      //WatchListItems.Clear();
      foreach (string tickerSymbol in watchItemsList)
      {
        decimal? currentPrice;
        try
        {
          currentPrice = marketFeedService.GetPrice(tickerSymbol);
        }
        catch (ArgumentException)
        {
          currentPrice = null;
        }

        WatchListItems.Add(new WatchItem(tickerSymbol, currentPrice));
      }
    }

    private void WatchListItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Add)
      {
        regionManager.Regions[RegionNames.MainRegion].RequestNavigate("/WatchListView", nr => { });
      }
    }
  }
}
