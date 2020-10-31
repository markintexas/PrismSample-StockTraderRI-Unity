using System;
using System.Windows.Input;

using Prism.Mvvm;

using StockTraderRI.Modules.Watch.Services;

namespace StockTraderRI.Modules.Watch.AddWatch
{
  public class AddWatchViewModel : BindableBase
  {
    private string stockSymbol;
    private IWatchListService watchListService;

    public AddWatchViewModel(IWatchListService watchListService)
    {
      this.watchListService = watchListService ?? throw new ArgumentNullException("watchListService");
    }

    public string StockSymbol
    {
      get { return stockSymbol; }
      set
      {
        SetProperty(ref stockSymbol, value);
      }
    }

    public ICommand AddWatchCommand { get { return watchListService.AddWatchCommand; } }
  }
}
