using System.Windows.Controls;
using System.Windows.Media.Animation;

using StockTraderRI.Modules.News.Controllers;

namespace StockTraderRI.Modules.News.Article
{
  public partial class ArticleView : UserControl
  {
    // Note - this import is here so that the controller is created and gets wired to the article and news reader
    // view models, which are shared instances
    private INewsController NewsController { get; set; }

    public ArticleView(ArticleViewModel viewModel, INewsController newsController)
    {
      InitializeComponent();
      ViewModel = viewModel;
      NewsController = newsController;
    }

    ArticleViewModel ViewModel
    {
      set
      {
        DataContext = value;
      }
    }


    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (NewsList.SelectedItem != null)
      {
        var storyboard = (Storyboard)Resources["Details"];
        storyboard.Begin();
      }
      else
      {
        var storyboard = (Storyboard)Resources["List"];
        storyboard.Begin();
      }
    }
  }
}
