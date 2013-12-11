using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{
	public interface ISimpleCell{
		void Bind(string title, string subtitle);
		void SetImage(UIImage image);
		string ImageUrl{get;set;}

		float TableWidth { get; set;}
	}
	
}
