using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;

using Prism.Commands;
using Prism.Events;

using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Messaging;

namespace StockTraderRI.Modules.Watch.Services
{
  public class WatchListService : IWatchListService
  {
    private readonly IMarketFeedService marketFeedService;
    private readonly IEventAggregator eventAggregator;

    //private ObservableCollection<string> WatchItems { get; set; }

    public WatchListService(IEventAggregator eventAggregator, IMarketFeedService marketFeedService)
    {
      this.marketFeedService = marketFeedService;
      this.eventAggregator = eventAggregator;

      //WatchItems = new ObservableCollection<string>();

      AddWatchCommand = new DelegateCommand<string>(AddWatch);
    }

    //public ObservableCollection<string> RetrieveWatchList()
    //{
    //  return WatchItems;
    //}

    private void AddWatch(string tickerSymbol)
    {
      if (!String.IsNullOrEmpty(tickerSymbol))
      {
        string upperCasedTrimmedSymbol = tickerSymbol.ToUpper(CultureInfo.InvariantCulture).Trim();
        //if (!WatchItems.Contains(upperCasedTrimmedSymbol))
        {
          if (marketFeedService.SymbolExists(upperCasedTrimmedSymbol))
          {
            eventAggregator.GetEvent<AddWatchStockEvent>().Publish(upperCasedTrimmedSymbol);       // This stock wants to be watched
            //WatchItems.Add(upperCasedTrimmedSymbol);
          }
        }
      }
    }

    public ICommand AddWatchCommand { get; set; }
  }
}
