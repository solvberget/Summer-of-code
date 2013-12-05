using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public interface ISimpleCellBinder<T> where T : class
	{
		void Bind(SimpleCell cell, T model);
	}
}
