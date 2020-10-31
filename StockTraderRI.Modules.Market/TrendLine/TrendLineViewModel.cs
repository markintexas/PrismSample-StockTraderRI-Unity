using System;
using Prism.Mvvm;
using Prism.Events;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Infrastructure;

namespace StockTraderRI.Modules.Market.TrendLine
{
    public class TrendLineViewModel : BindableBase
    {
        private readonly IMarketHistoryService marketHistoryService;

        private string tickerSymbol;

        private MarketHistoryCollection historyCollection;

        public TrendLineViewModel(IMarketHistoryService marketHistoryService, IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            this.marketHistoryService = marketHistoryService;
            eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Subscribe(TickerSymbolChanged);
        }

        public void TickerSymbolChanged(string newTickerSymbol)
        {
            MarketHistoryCollection newHistoryCollection = marketHistoryService.GetPriceHistory(newTickerSymbol);

            TickerSymbol = newTickerSymbol;
            HistoryCollection = newHistoryCollection;
        }

        public string TickerSymbol
        {
            get
            {
                return tickerSymbol;
            }
            set
            {
                SetProperty(ref tickerSymbol, value);
            }
        }

        public MarketHistoryCollection HistoryCollection
        {
            get
            {
                return historyCollection;
            }
            private set
            {
                SetProperty(ref historyCollection, value);
            }
        }
    }
}
