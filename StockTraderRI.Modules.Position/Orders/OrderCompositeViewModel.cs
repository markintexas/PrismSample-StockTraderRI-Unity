using System;
using System.Windows;
using System.Windows.Input;

using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.Position.Models;

namespace StockTraderRI.Modules.Position.Orders
{
  public partial class OrderCompositeViewModel : DependencyObject, IOrderCompositeViewModel, IHeaderInfoProvider<string>
  {
    private readonly IOrderDetailsViewModel orderDetailsViewModel;

    public static readonly DependencyProperty HeaderInfoProperty =
        DependencyProperty.Register("HeaderInfo", typeof(string), typeof(OrderCompositeViewModel), null);

    public OrderCompositeViewModel(IOrderDetailsViewModel orderDetailsViewModel)
    {
      this.orderDetailsViewModel = orderDetailsViewModel ?? throw new ArgumentNullException("orderDetailsViewModel");
      this.orderDetailsViewModel.CloseViewRequested += OrderViewModel_CloseViewRequested;
    }

    void OrderViewModel_CloseViewRequested(object sender, EventArgs e)
    {
      OnCloseViewRequested(sender, e);
    }

    partial void SetTransactionInfo(TransactionInfo transactionInfo);

    private void OnCloseViewRequested(object sender, EventArgs e)
    {
      CloseViewRequested(sender, e);
    }

    public event EventHandler CloseViewRequested = delegate { };

    public TransactionInfo TransactionInfo
    {
      get { return orderDetailsViewModel.TransactionInfo; }
      set { SetTransactionInfo(value); }

    }

    public ICommand SubmitCommand
    {
      get { return orderDetailsViewModel.SubmitCommand; }
    }

    public ICommand CancelCommand
    {
      get { return orderDetailsViewModel.CancelCommand; }
    }

    public int Shares
    {
      get { return orderDetailsViewModel.Shares ?? 0; }
    }

    public object OrderDetails
    {
      get { return orderDetailsViewModel; }
    }
  }
}