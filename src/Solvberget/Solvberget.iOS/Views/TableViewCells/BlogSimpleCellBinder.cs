using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Solvberget.Core.ViewModels;

namespace Solvberget.iOS
{

	public class BlogSimpleCellBinder : ISimpleCellBinder<BlogItemViewModel>
	{
		public void Bind(SimpleCell cell, BlogItemViewModel blog)
		{
			cell.Bind(blog.Title, blog.Description, null);
		}
	}
}
