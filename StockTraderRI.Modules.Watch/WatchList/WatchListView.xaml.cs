using System.Diagnostics.CodeAnalysis;
using System.Windows.Controls;

namespace StockTraderRI.Modules.Watch.WatchList
{
  public partial class WatchListView : UserControl
  {
    //public WatchListView(WatchListViewModel viewModel)
    public WatchListView()
    {
      InitializeComponent();
      //    ViewModel = viewModel;
    }

    //[SuppressMessage("Microsoft.Design", "CA1044:PropertiesShouldNotBeWriteOnly", Justification = "Needs to be a property to be composed by MEF")]
    //WatchListViewModel ViewModel
    //{
    //  set
    //  {
    //    DataContext = value;
    //  }
    //}
  }
}
