using System;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;
using MonoTouch.Foundation;

namespace Solvberget.iOS
{
	public class LabelAndValue
    {
		UILabel _label = new UILabel();
		UILabel _value = new UILabel();

		public LabelAndValue(UIView container, string label, string value, Action onTap = null)
			: this(container, label, value, false, onTap)
		{}

		public LabelAndValue(UIView container, string label, string value, bool bold, Action onTap = null)
        {
			_label.Font = Application.ThemeColors.LabelFont;
			_label.TextColor = Application.ThemeColors.Main;

			_value.Font = Application.ThemeColors.DefaultFont;
			_value.TextColor = Application.ThemeColors.Main2;
			_value.Lines = 0;
			_value.LineBreakMode = UILineBreakMode.WordWrap;

			if (bold) _value.Font = Application.ThemeColors.DefaultFontBold;

			var prev = container.Subviews.Length == 0 ? RectangleF.Empty : container.Subviews.Last().Frame;

			if (String.IsNullOrEmpty(label) && bold)
			{
				_value.Font = Application.ThemeColors.HeaderFont;
			}
			else
			{
				container.Add(_label);
				_label.Text = null == label ? String.Empty : label.ToUpperInvariant();
			}

			if (!String.IsNullOrEmpty(value))
			{
				container.Add(_value);
				_value.Text = value;
			}

			var padding = 10f;

			var labelSize = _label.SizeThatFits(new SizeF(container.Frame.Width - 2*padding, 0));
			var valueSize = _value.SizeThatFits(new SizeF(container.Frame.Width - 2*padding, 0));

			_label.Frame = new RectangleF(new PointF(padding,prev.Bottom+padding), labelSize);

			var y = _label.Frame.Bottom;
			_value.Frame = new RectangleF(new PointF(padding, y), valueSize);

			container.Frame = new RectangleF(container.Frame.Location, 
				new SizeF(container.Frame.Width, container.Frame.Height + padding + _label.Frame.Height + _value.Frame.Height));

			if (null != onTap)
			{
				_value.TextColor = Application.ThemeColors.Main;
				var onTapAction = new NSAction(onTap);

				_label.AddGestureRecognizer(new UITapGestureRecognizer(onTapAction));
				_value.AddGestureRecognizer(new UITapGestureRecognizer(onTapAction));

				_label.UserInteractionEnabled = _value.UserInteractionEnabled = true;
			}

		
		}
    }
}

