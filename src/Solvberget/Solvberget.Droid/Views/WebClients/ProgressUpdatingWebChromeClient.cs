using Android.Webkit;
using Android.Widget;

namespace Solvberget.Droid.Views.WebClients
{
    public class ProgressUpdatingWebChromeClient : WebChromeClient
    {
        private readonly ProgressBar _progressBar;

        public ProgressUpdatingWebChromeClient(ProgressBar progressBar)
        {
            _progressBar = progressBar;
        }

        public override void OnProgressChanged(WebView view, int newProgress)
        {
            _progressBar.Progress = newProgress;
        }
    }
}