using Android.App;
using Android.Content;

namespace Solvberget.Droid.Views
{
    public class LoadingIndicator
    {
        private readonly Context _context;
        private ProgressDialog _dialog;

        public LoadingIndicator(Context context)
        {
            _context = context;
        }

        public bool Visible
        {
            get { return _dialog != null; }
            set
            {
                if (value == Visible)
                    return;

                if (value)
                {
                    _dialog = new ProgressDialog(_context);
                    _dialog.SetCanceledOnTouchOutside(false);
                    _dialog.SetTitle("Laster...");
                    _dialog.Show();
                }
                else
                {
                    _dialog.Hide();
                    _dialog = null;
                }
            }
        }
    }
}