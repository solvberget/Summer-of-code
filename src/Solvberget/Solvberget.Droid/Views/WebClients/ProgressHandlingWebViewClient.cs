using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace Solvberget.Droid.Views.WebClients
{
    public class ProgressHandlingWebViewClient : WebViewClient
    {
        private readonly ProgressBar _progressBar;

        public ProgressHandlingWebViewClient(ProgressBar progressBar)
        {
            _progressBar = progressBar;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            _progressBar.Visibility = ViewStates.Gone;
            base.OnPageFinished(view, url);
        }
    }
}