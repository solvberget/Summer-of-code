using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Solvberget.Core.ViewModels;
using Solvberget.Droid.ActionBar;

namespace Solvberget.Droid.Helpers
{
    public interface IFragmentHost
    {
        bool Show(MvxViewModelRequest request);
    }

    public interface ICustomPresenter
    {
        void Register(Type viewModelType, IFragmentHost host);
    }

    public class CustomPresenter
        : MvxAndroidViewPresenter
        , ICustomPresenter
    {
        private readonly Dictionary<Type, IFragmentHost> _dictionary = new Dictionary<Type, IFragmentHost>();

        public override void Show(MvxViewModelRequest request)
        {
            IFragmentHost host;
            if (_dictionary.TryGetValue(request.ViewModelType, out host))
            {
                if (host.Show(request))
                {
                    return;
                }
            }

            base.Show(request);
        }

        public void Register(Type viewModelType, IFragmentHost host)
        {
            _dictionary[viewModelType] = host;
        }

        public override void Close(IMvxViewModel viewModel)
        {
            if (viewModel is LoginViewModel)
            {
                var act = Activity as MvxActionBarActivity;
                if (act != null)
                {
                    act.SupportFragmentManager.PopBackStackImmediate();
                }
            }
            else
            {
                base.Close(viewModel);
            }
            
        }
    }

}