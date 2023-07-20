#if IOS && !MACCATALYST
using System;
using CoreGraphics;
using UIKit;

namespace Microsoft.Maui.Platform
{
	internal class MauiDoneAccessoryView : UIToolbar
	{
		readonly WeakReference<Action>? _doneClicked;
		WeakReference<object>? _data;
		WeakReference<Action<object>>? _doneWithDataClicked;

		public MauiDoneAccessoryView() : base(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44))
		{
			BarStyle = UIBarStyle.Default;
			Translucent = true;
			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, OnDataClicked);

			SetItems(new[] { spacer, doneButton }, false);
		}

		void OnDataClicked(object? sender, EventArgs e)
		{
			if (_doneWithDataClicked is not null && _doneWithDataClicked.TryGetTarget(out var handler))
			{
				if (_data is not null && _data.TryGetTarget(out var data))
					handler(data);
			}
		}

		void OnClicked(object? sender, EventArgs e)
		{
			if (_doneClicked is not null && _doneClicked.TryGetTarget(out var handler))
			{
				handler();
			}
		}

		internal void SetDoneClicked(Action<object>? value) => _doneWithDataClicked = value is null ? null : new(value);
		internal void SetDataContext(object? dataContext)
		{
			_data = null;
			if (dataContext == null)
				return;

			_data = new WeakReference<object>(dataContext);
		}

		public MauiDoneAccessoryView(Action doneClicked) : base(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44))
		{
			_doneClicked = new(doneClicked);
			BarStyle = UIBarStyle.Default;
			Translucent = true;

			var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, OnClicked);
			SetItems(new[] { spacer, doneButton }, false);
		}
	}
}
#endif