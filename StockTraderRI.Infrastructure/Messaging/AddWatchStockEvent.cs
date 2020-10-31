using Prism.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTraderRI.Infrastructure.Messaging
{
  public class AddWatchStockEvent : PubSubEvent<string>
  {
  }
}
