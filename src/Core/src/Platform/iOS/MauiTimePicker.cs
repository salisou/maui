using System;
using Foundation;
using UIKit;
using RectangleF = CoreGraphics.CGRect;

namespace Microsoft.Maui.Platform
{
	public class MauiTimePicker : NoCaretField
	{
		readonly UIDatePicker _picker;

#if !MACCATALYST
		// NOTE: keep the Action alive as long as MauiTimePicker
		readonly Action _onDone;
#endif

		public MauiTimePicker()
		{
			BorderStyle = UITextBorderStyle.RoundedRect;

			_picker = new UIDatePicker { Mode = UIDatePickerMode.Time, TimeZone = new NSTimeZone("UTC") };

			if (OperatingSystem.IsIOSVersionAtLeast(14))
			{
				_picker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
			}

			InputView = _picker;

#if !MACCATALYST
			InputAccessoryView = new MauiDoneAccessoryView(_onDone = () =>
			{
				DateSelected?.Invoke(this, EventArgs.Empty);
				ResignFirstResponder();
			});

			InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
#endif

			InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			InputAssistantItem.LeadingBarButtonGroups = null;
			InputAssistantItem.TrailingBarButtonGroups = null;

			AccessibilityTraits = UIAccessibilityTrait.Button;
		}

		public UIDatePicker Picker => _picker;

		public NSDate Date => Picker.Date;

#if !MACCATALYST
		public event EventHandler? DateSelected;
#endif
		public void UpdateTime(TimeSpan time)
		{
			_picker.Date = new DateTime(1, 1, 1, time.Hours, time.Minutes, time.Seconds).ToNSDate();
		}
	}
}