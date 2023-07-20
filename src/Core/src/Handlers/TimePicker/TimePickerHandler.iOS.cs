using System;

namespace Microsoft.Maui.Handlers
{
#if IOS && !MACCATALYST
	public partial class TimePickerHandler : ViewHandler<ITimePicker, MauiTimePicker>
	{
		protected override MauiTimePicker CreatePlatformView()
		{
			return new MauiTimePicker();
		}

		internal bool UpdateImmediately { get; set; }

		protected override void ConnectHandler(MauiTimePicker platformView)
		{
			base.ConnectHandler(platformView);

			platformView?.UpdateTime(VirtualView.Time);
		}

		protected override void DisconnectHandler(MauiTimePicker platformView)
		{
			base.DisconnectHandler(platformView);

			platformView?.RemoveFromSuperview();
		}

		public override void OnWindowChanged(object? oldValue, object? newValue)
		{
			var platformView = PlatformView;
			if (platformView is null)
				return;

			if (newValue is null)
			{
				platformView.EditingDidBegin += OnStarted;
				platformView.EditingDidEnd += OnEnded;
				platformView.ValueChanged += OnValueChanged;
				platformView.DateSelected += OnDateSelected;
				platformView.Picker.ValueChanged += OnValueChanged;
			}
			else
			{
				platformView.EditingDidBegin -= OnStarted;
				platformView.EditingDidEnd -= OnEnded;
				platformView.ValueChanged -= OnValueChanged;
				platformView.DateSelected -= OnDateSelected;
				platformView.Picker.ValueChanged -= OnValueChanged;
			}
		}

		public static void MapFormat(ITimePickerHandler handler, ITimePicker timePicker)
		{
			handler.PlatformView?.UpdateFormat(timePicker, handler.PlatformView?.Picker);
		}

		public static void MapTime(ITimePickerHandler handler, ITimePicker timePicker)
		{
			handler.PlatformView?.UpdateTime(timePicker, handler.PlatformView?.Picker);
		}

		public static void MapCharacterSpacing(ITimePickerHandler handler, ITimePicker timePicker)
		{
			handler.PlatformView?.UpdateCharacterSpacing(timePicker);
		}

		public static void MapFont(ITimePickerHandler handler, ITimePicker timePicker)
		{
			var fontManager = handler.GetRequiredService<IFontManager>();

			handler.PlatformView?.UpdateFont(timePicker, fontManager);
		}

		public static void MapTextColor(ITimePickerHandler handler, ITimePicker timePicker)
		{
			handler.PlatformView?.UpdateTextColor(timePicker);
		}

		public static void MapFlowDirection(TimePickerHandler handler, ITimePicker timePicker)
		{
			handler.PlatformView?.UpdateFlowDirection(timePicker);
			handler.PlatformView?.UpdateTextAlignment(timePicker);
		}

		void OnStarted(object? sender, EventArgs eventArgs)
		{
			if (VirtualView is not null)
				VirtualView.IsFocused = true;
		}

		void OnEnded(object? sender, EventArgs eventArgs)
		{
			if (VirtualView is not null)
				VirtualView.IsFocused = false;
		}

		void OnValueChanged(object? sender, EventArgs e)
		{
			if (UpdateImmediately)  // Platform Specific
				SetVirtualViewTime();
		}

		void OnDateSelected(object? sender, EventArgs e)
		{
			SetVirtualViewTime();
		}

		void SetVirtualViewTime()
		{
			if (VirtualView == null || PlatformView == null)
				return;

			var datetime = PlatformView.Date.ToDateTime();
			VirtualView.Time = new TimeSpan(datetime.Hour, datetime.Minute, 0);
		}
	}
#endif
}