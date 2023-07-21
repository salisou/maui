using System;
using Foundation;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public class MauiTimePicker : NoCaretField
	{
		readonly UIDatePicker _picker;
		readonly WeakReference<ITimePicker> _virtualView;

#if !MACCATALYST
		// NOTE: keep the Action alive as long as MauiTimePicker
		readonly Action _onDone;
#endif

		public MauiTimePicker(ITimePicker virtualView)
		{
			_virtualView = new(virtualView);
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
				SetVirtualViewTime();
				ResignFirstResponder();
			});

			InputAccessoryView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;
#endif

			InputView.AutoresizingMask = UIViewAutoresizing.FlexibleHeight;

			InputAssistantItem.LeadingBarButtonGroups = null;
			InputAssistantItem.TrailingBarButtonGroups = null;

			AccessibilityTraits = UIAccessibilityTrait.Button;

			EditingDidBegin += OnEditingDidBegin;
			EditingDidEnd += OnEditingDidEnd;
			ValueChanged += OnValueChanged;
			_picker.ValueChanged += OnValueChanged;
		}

		static void OnEditingDidBegin(object? sender, EventArgs e)
		{
			if (sender is MauiTimePicker @this && @this._virtualView.TryGetTarget(out var v))
				v.IsFocused = true;
		}

		static void OnEditingDidEnd(object? sender, EventArgs e)
		{
			if (sender is MauiTimePicker @this && @this._virtualView.TryGetTarget(out var v))
				v.IsFocused = false;
		}

		static void OnValueChanged(object? sender, EventArgs e)
		{
			if (sender is MauiTimePicker @this && @this.UpdateImmediately)  // Platform Specific
				@this.SetVirtualViewTime();
		}

		static void OnDateSelected(object? sender, EventArgs e)
		{
			if (sender is MauiTimePicker @this)
				@this.SetVirtualViewTime();
		}

		void SetVirtualViewTime()
		{
			if (_virtualView.TryGetTarget(out var v))
			{
				var datetime = Date.ToDateTime();
				v.Time = new TimeSpan(datetime.Hour, datetime.Minute, 0);
			}
		}

		public UIDatePicker Picker => _picker;

		public NSDate Date => Picker.Date;

		internal bool UpdateImmediately { get; set; }

		public void UpdateTime(TimeSpan time)
		{
			_picker.Date = new DateTime(1, 1, 1, time.Hours, time.Minutes, time.Seconds).ToNSDate();
		}
	}
}