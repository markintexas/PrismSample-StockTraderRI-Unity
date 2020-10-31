using Prism.Commands;

namespace StockTraderRI.Infrastructure
{

    public static class StockTraderRICommands
    {
    public static CompositeCommand SubmitOrderCommand { get; set; } = new CompositeCommand(true);

    public static CompositeCommand CancelOrderCommand { get; set; } = new CompositeCommand(true);

    public static CompositeCommand SubmitAllOrdersCommand { get; set; } = new CompositeCommand();

    public static CompositeCommand CancelAllOrdersCommand { get; set; } = new CompositeCommand();
  }

    public class StockTraderRICommandProxy
    {
        virtual public CompositeCommand SubmitOrderCommand
        {
            get { return StockTraderRICommands.SubmitOrderCommand; }
        }

        virtual public CompositeCommand CancelOrderCommand
        {
            get { return StockTraderRICommands.CancelOrderCommand; }
        }

        virtual public CompositeCommand SubmitAllOrdersCommand
        {
            get { return StockTraderRICommands.SubmitAllOrdersCommand; }
        }

        virtual public CompositeCommand CancelAllOrdersCommand
        {
            get { return StockTraderRICommands.CancelAllOrdersCommand; }
        }
    }
}
