using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

using Prism.Commands;
using Prism.Mvvm;

using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using StockTraderRI.Modules.Position.Interfaces;
using StockTraderRI.Modules.Position.Models;
using StockTraderRI.Modules.Position.Properties;

namespace StockTraderRI.Modules.Position.Orders
{
  public class OrderDetailsViewModel : BindableBase, IOrderDetailsViewModel
  {
    private readonly IAccountPositionService accountPositionService;
    private readonly IOrdersService ordersService;
    private TransactionInfo transactionInfo;
    private int? shares;
    private OrderType orderType = OrderType.Market;
    private decimal? stopLimitPrice;
    private TimeInForce timeInForce;

    private readonly List<string> errors = new List<string>();

    public OrderDetailsViewModel(IAccountPositionService accountPositionService, IOrdersService ordersService)
    {
      this.accountPositionService = accountPositionService;
      this.ordersService = ordersService;

      transactionInfo = new TransactionInfo();
      
      //use localizable enum descriptions
      AvailableOrderTypes = new ValueDescriptionList<OrderType>
      {
          new ValueDescription<OrderType>(OrderType.Limit, Resources.OrderType_Limit),
          new ValueDescription<OrderType>(OrderType.Market, Resources.OrderType_Market),
          new ValueDescription<OrderType>(OrderType.Stop, Resources.OrderType_Stop)
      };

      AvailableTimesInForce = new ValueDescriptionList<TimeInForce>
      {
          new ValueDescription<TimeInForce>(TimeInForce.EndOfDay, Resources.TimeInForce_EndOfDay),
          new ValueDescription<TimeInForce>(TimeInForce.ThirtyDays, Resources.TimeInForce_ThirtyDays)
      };

      //SubmitCommand = new DelegateCommand<object>(Submit, CanSubmit);
      SubmitCommand = new DelegateCommand<object>(Submit, CanSubmit).ObservesProperty(() => HasErrors);
      CancelCommand = new DelegateCommand<object>(Cancel);

      SetInitialValidState();
    }

    public event EventHandler CloseViewRequested = delegate { };

    public IValueDescriptionList<OrderType> AvailableOrderTypes { get; private set; }

    public IValueDescriptionList<TimeInForce> AvailableTimesInForce { get; private set; }

    public TransactionInfo TransactionInfo
    {
      get { return transactionInfo; }
      set
      {
        SetProperty(ref transactionInfo, value);
        //this.OnPropertyChanged(() => TickerSymbol);
        RaisePropertyChanged("TickerSymbol");
      }
    }

    public TransactionType TransactionType
    {
      get { return transactionInfo.TransactionType; }
      set
      {
        ValidateHasEnoughSharesToSell(Shares, value, false);

        if (transactionInfo.TransactionType != value)
        {
          transactionInfo.TransactionType = value;
          RaisePropertyChanged("TransactionType");

          //OnPropertyChanged( new PropertyChangedEventArgs("TransactionType"));
        }
      }
    }

    public string TickerSymbol
    {
      get { return transactionInfo.TickerSymbol; }
      set
      {
        if (transactionInfo.TickerSymbol != value)
        {
          transactionInfo.TickerSymbol = value;
          RaisePropertyChanged("TickerSymbol");
        }
      }
    }

    public int? Shares
    {
      get { return shares; }
      set
      {
        ValidateShares(value, true);
        ValidateHasEnoughSharesToSell(value, TransactionType, true);

        SetProperty(ref shares, value);
      }
    }

    public OrderType OrderType
    {
      get { return orderType; }
      set
      {
        SetProperty(ref orderType, value);
      }
    }

    public decimal? StopLimitPrice
    {
      get
      {
        return stopLimitPrice;
      }
      set
      {
        ValidateStopLimitPrice(value, true);

        SetProperty(ref stopLimitPrice, value);
      }
    }

    public TimeInForce TimeInForce
    {
      get { return timeInForce; }
      set
      {
        SetProperty(ref timeInForce, value);
      }
    }

    private bool hasErrors;
    
    /// <summary>
    /// True if there are errors, false otherwise
    /// </summary>
    public bool HasErrors
    {
      get { return hasErrors; }
      set { SetProperty(ref hasErrors, value); }
    }

    public DelegateCommand<object> SubmitCommand { get; private set; }

    public DelegateCommand<object> CancelCommand { get; private set; }

    private void SetInitialValidState()
    {
      ValidateShares(Shares, false);
      ValidateStopLimitPrice(StopLimitPrice, false);
    }

    private void ValidateShares(int? newSharesValue, bool throwException)
    {
      if (!newSharesValue.HasValue || newSharesValue.Value <= 0)
      {
        AddError("InvalidSharesRange");
        if (throwException)
        {
          throw new InputValidationException(Resources.InvalidSharesRange);
        }
      }
      else
      {
        RemoveError("InvalidSharesRange");
      }
    }

    private void ValidateStopLimitPrice(decimal? price, bool throwException)
    {
      if (!price.HasValue || price.Value <= 0)
      {
        AddError("InvalidStopLimitPrice");
        if (throwException)
        {
          throw new InputValidationException(Resources.InvalidStopLimitPrice);
        }
      }
      else
      {
        RemoveError("InvalidStopLimitPrice");
      }
    }

    private void ValidateHasEnoughSharesToSell(int? sharesToSell, TransactionType transactionType, bool throwException)
    {
      if (transactionType == TransactionType.Sell && !HoldsEnoughShares(TickerSymbol, sharesToSell))
      {
        AddError("NotEnoughSharesToSell");
        if (throwException)
        {
          throw new InputValidationException(String.Format(CultureInfo.InvariantCulture, Resources.NotEnoughSharesToSell, sharesToSell));
        }
      }
      else
      {
        RemoveError("NotEnoughSharesToSell");
      }
    }

    private void AddError(string ruleName)
    {
      if (!errors.Contains(ruleName))
      {
        errors.Add(ruleName);
        HasErrors = errors.Count > 0;
        //SubmitCommand.RaiseCanExecuteChanged();
      }
    }

    private void RemoveError(string ruleName)
    {
      if (errors.Contains(ruleName))
      {
        errors.Remove(ruleName);
        HasErrors = errors.Count != 0;
        //if (errors.Count == 0)
        //{
        //  SubmitCommand.RaiseCanExecuteChanged();
        //}
      }
    }

    private bool CanSubmit(object parameter)
    {
      return !HasErrors; // errors.Count == 0;
    }

    private bool HoldsEnoughShares(string symbol, int? sharesToSell)
    {
      if (!sharesToSell.HasValue)
      {
        return false;
      }

      foreach (AccountPosition accountPosition in accountPositionService.GetAccountPositions())
      {
        if (accountPosition.TickerSymbol.Equals(symbol, StringComparison.OrdinalIgnoreCase))
        {
          if (accountPosition.Shares >= sharesToSell)
          {
            return true;
          }
          else
          {
            return false;
          }
        }
      }
      return false;
    }

    private void Submit(object parameter)
    {
      if (!CanSubmit(parameter))
      {
        throw new InvalidOperationException();
      }

      var order = new Order
      {
        TransactionType = TransactionType,
        OrderType = OrderType,
        Shares = Shares.Value,
        StopLimitPrice = StopLimitPrice.Value,
        TickerSymbol = TickerSymbol,
        TimeInForce = TimeInForce
      };

      ordersService.Submit(order);

      CloseViewRequested(this, EventArgs.Empty);
    }

    private void Cancel(object parameter)
    {
      CloseViewRequested(this, EventArgs.Empty);
    }
  }
}
