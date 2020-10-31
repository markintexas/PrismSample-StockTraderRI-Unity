using System.Diagnostics;
using System.Windows.Controls;

namespace StockTraderRI.Modules.Position.Orders
{
  public partial class OrderDetailsView : UserControl
  {
    public OrderDetailsView()
    {
      Debug.WriteLine("OrderDetailsView");
      InitializeComponent();
    }
  }
}