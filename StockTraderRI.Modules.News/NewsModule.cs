//using Microsoft.Practices.Unity;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Modules.News.Article;
using StockTraderRI.Modules.News.Controllers;
using StockTraderRI.Modules.News.Services;

using System.ComponentModel;

namespace StockTraderRI.Modules.News
{
    public class NewsModule : IModule
    {
        private readonly IContainerProvider container;
        private readonly IRegionManager regionManager;

        public NewsModule(IContainerProvider container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        //public void Initialize()
        //{
        //    this.container.RegisterInstance(typeof(INewsFeedService),
        //                                    container.Resolve<NewsFeedService>());

        //    this.container.RegisterInstance(typeof(ArticleViewModel),
        //                                    container.Resolve<ArticleViewModel>());

        //    this.container.RegisterInstance(typeof(NewsReaderViewModel),
        //                                    container.Resolve<NewsReaderViewModel>());

        //    this.container.RegisterInstance(typeof(INewsController),
        //                                    container.Resolve<NewsController>());

        //    this.regionManager.RegisterViewWithRegion(RegionNames.ResearchRegion,
        //                                               () => this.container.Resolve<ArticleView>());

        //    this.regionManager.RegisterViewWithRegion(RegionNames.SecondaryRegion,
        //                                               () => this.container.Resolve<NewsReaderView>());
        //}

    public void OnInitialized(Prism.Ioc.IContainerProvider containerProvider)
    {
      regionManager.RegisterViewWithRegion(RegionNames.ResearchRegion,
                                                 () => container.Resolve<ArticleView>());

      regionManager.RegisterViewWithRegion(RegionNames.SecondaryRegion,
                                                 () => container.Resolve<NewsReaderView>());
    }

    public void RegisterTypes(Prism.Ioc.IContainerRegistry containerRegistry)
    {
      //this.container.RegisterInstance(typeof(INewsFeedService),
      //                                container.Resolve<NewsFeedService>());
      containerRegistry.Register<INewsFeedService>(() => container.Resolve<NewsFeedService>());

      //this.container.RegisterInstance(typeof(ArticleViewModel),
      //                                container.Resolve<ArticleViewModel>());
      //containerRegistry.Register<ArticleViewModel>(() => container.Resolve<ArticleViewModel>());
      containerRegistry.Register<ArticleViewModel>();

      //this.container.RegisterInstance(typeof(NewsReaderViewModel),
      //                                container.Resolve<NewsReaderViewModel>());
      //containerRegistry.Register<NewsReaderViewModel>(() => container.Resolve<NewsReaderViewModel>());
      containerRegistry.Register<NewsReaderViewModel>();

      //this.container.RegisterInstance(typeof(INewsController),
      //                                container.Resolve<NewsController>());
      //containerRegistry.Register<INewsController>(() => container.Resolve<NewsController>());
      containerRegistry.Register<INewsController, NewsController>();
    }
  }
}
