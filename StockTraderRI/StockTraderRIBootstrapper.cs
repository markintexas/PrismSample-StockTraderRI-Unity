using StockTraderRI.Modules.Market;
using StockTraderRI.Modules.News;
using StockTraderRI.Modules.Position;
using StockTraderRI.Modules.Watch;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using StockTraderRI.Views;

using System.Windows;

namespace StockTraderRI
{
    public class StockTraderRIBootstrapper // : UnityBootstrapper
    {
        //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        //{
        //    base.ConfigureModuleCatalog();

        //    moduleCatalog.AddModule<MarketModule>();
        //    moduleCatalog.AddModule<PositionModule));
        //    moduleCatalog.AddModule<WatchModule));
        //    moduleCatalog.AddModule<NewsModule));
        //}

        //protected override DependencyObject CreateShell()
        //{
        //    // Use the container to create an instance of the shell.
        //    Shell view = this.Container.TryResolve<Shell>();
        //    view.DataContext = new ShellViewModel();
        //    return view;
        //}

        //protected override void InitializeShell()
        //{
        //    base.InitializeShell();
        //    App.Current.MainWindow = (Window)this.Shell;
        //    App.Current.MainWindow.Show();
        //}

        //protected override Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        //{
        //    var factory = base.ConfigureDefaultRegionBehaviors();
        //    return factory;
        //}
    }
}
