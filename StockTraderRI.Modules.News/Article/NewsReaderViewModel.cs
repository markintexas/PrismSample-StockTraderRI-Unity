using Prism.Mvvm;
using Prism.Services.Dialogs;

using StockTraderRI.Infrastructure.Models;

using System;

namespace StockTraderRI.Modules.News.Article
{
  public class NewsReaderViewModel : BindableBase, IDialogAware
  {
    private NewsArticle newsArticle;
    public NewsArticle NewsArticle
    {
      get
      {
        return newsArticle;
      }
      set
      {
        SetProperty(ref newsArticle, value);
      }
    }

    public string Title => NewsArticle?.Title;

    public event Action<IDialogResult> RequestClose;

    protected virtual void CloseDialog(string parameter)
    {
      ButtonResult result = ButtonResult.None;

      if (parameter?.ToLower() == "true")
        result = ButtonResult.OK;
      else if (parameter?.ToLower() == "false")
        result = ButtonResult.Cancel;

      RaiseRequestClose(new DialogResult(result));
    }

    public virtual void RaiseRequestClose(IDialogResult dialogResult)
    {
      RequestClose?.Invoke(dialogResult);
    }

    public bool CanCloseDialog()
    {
      return true;
    }

    public void OnDialogClosed()
    {
      //throw new NotImplementedException();
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
      NewsArticle = parameters.GetValue<NewsArticle>("NewsArticle");
    }
  }
}
