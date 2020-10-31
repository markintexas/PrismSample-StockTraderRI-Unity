

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Events;
using Prism.Regions;
using StockTraderRI.Infrastructure;
using StockTraderRI.Infrastructure.Interfaces;
using StockTraderRI.Infrastructure.Models;
using Prism.Services.Dialogs;

namespace StockTraderRI.Modules.News.Article
{
  public class ArticleViewModel : BindableBase
  {
    private string _companySymbol;
    private IList<NewsArticle> articles;
    private NewsArticle selectedArticle;
    private readonly INewsFeedService newsFeedService;
    private readonly IRegionManager regionManager;

    public ArticleViewModel(IRegionManager regionManager)
    {
      this.regionManager = regionManager;
    }

    private readonly ICommand showArticleListCommand;
    private readonly ICommand showNewsReaderViewCommand;
    private readonly IDialogService dialogService;

    public ArticleViewModel(INewsFeedService newsFeedService, IRegionManager regionManager, IEventAggregator eventAggregator, IDialogService dialogService)
    {
      if (eventAggregator == null)
      {
        throw new ArgumentNullException("eventAggregator");
      }

      this.newsFeedService = newsFeedService ?? throw new ArgumentNullException("newsFeedService");
      this.regionManager = regionManager ?? throw new ArgumentNullException("regionManager");
      this.dialogService = dialogService ?? throw new ArgumentNullException("diaogService");

      showArticleListCommand = new DelegateCommand(ShowArticleList);
      showNewsReaderViewCommand = new DelegateCommand(ShowNewsReaderView);

      eventAggregator.GetEvent<TickerSymbolSelectedEvent>().Subscribe(OnTickerSymbolSelected, ThreadOption.UIThread);
    }

    public string CompanySymbol
    {
      get
      {
        return _companySymbol;
      }
      set
      {
        if (SetProperty(ref _companySymbol, value))
        {
          OnCompanySymbolChanged();
        }
      }
    }

    public NewsArticle SelectedArticle
    {
      get { return selectedArticle; }
      set
      {
        SetProperty(ref selectedArticle, value);
      }
    }

    public IList<NewsArticle> Articles
    {
      get { return articles; }
      private set
      {
        SetProperty(ref articles, value);
      }
    }

    public ICommand ShowNewsReaderCommand { get { return showNewsReaderViewCommand; } }

    public ICommand ShowArticleListCommand { get { return showArticleListCommand; } }

    private void OnTickerSymbolSelected(string companySymbol)
    {
      CompanySymbol = companySymbol;
    }

    private void OnCompanySymbolChanged()
    {
      Articles = newsFeedService.GetNews(_companySymbol);
    }

    private void ShowArticleList()
    {
      SelectedArticle = null;
    }

    private void ShowNewsReaderView()
    {
      //regionManager.RequestNavigate(RegionNames.SecondaryRegion, new Uri("/NewsReaderView", UriKind.Relative));
      var dialogParameters = new DialogParameters
      {
        { "NewsArticle", SelectedArticle }
      };
      dialogService.ShowDialog("NewsReaderDialog", dialogParameters, (r) => { } ) ;
    }
  }
}
