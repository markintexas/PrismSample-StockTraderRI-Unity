using Prism.Mvvm;
namespace StockTraderRI.Modules.Position.PositionSummary
{
    public class PositionSummaryItem : BindableBase
    {
        public PositionSummaryItem(string tickerSymbol, decimal costBasis, long shares, decimal currentPrice)
        {
            TickerSymbol = tickerSymbol;
            CostBasis = costBasis;
            Shares = shares;
            CurrentPrice = currentPrice;
        }

        private string _tickerSymbol;

        public string TickerSymbol
        {
            get
            {
                return _tickerSymbol;
            }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                SetProperty(ref _tickerSymbol, value);
            }
        }


        private decimal _costBasis;

        public decimal CostBasis
        {
            get
            {
                return _costBasis;
            }
            set
            {
                if (SetProperty(ref _costBasis, value))
                {
          //          this.OnPropertyChanged(() => GainLossPercent);
          RaisePropertyChanged("GainLossPercent");
                }
            }
        }


        private long _shares;

        public long Shares
        {
            get
            {
                return _shares;
            }
            set
            {
                if (SetProperty(ref _shares, value))
                {
                    //this.OnPropertyChanged(() => MarketValue);
                    //this.OnPropertyChanged(() => GainLossPercent);
          RaisePropertyChanged("MarketValue");
          RaisePropertyChanged("GainLossPercent");
                }
            }
        }


        private decimal _currentPrice;

        public decimal CurrentPrice
        {
            get
            {
                return _currentPrice;
            }
            set
            {
                if (SetProperty(ref _currentPrice, value))
                {
                    //this.OnPropertyChanged(() => MarketValue);
                    //this.OnPropertyChanged(() => GainLossPercent);
          RaisePropertyChanged("MarketValue");
          RaisePropertyChanged("GainLossPercent");
        }
      }
        }

        public decimal MarketValue
        {
            get
            {
                return (_shares * _currentPrice);
            }
        }

        public decimal GainLossPercent
        {
            get
            {
                return ((CurrentPrice * Shares - CostBasis) * 100 / CostBasis);
            }
        }
    }
}