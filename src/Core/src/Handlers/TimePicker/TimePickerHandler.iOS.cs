using System;

namespace Microsoft.Maui.Handlers
{
#if IOS && !MACCATALYST
	public partial class TimePickerHandler : ViewHandler<ITimePicker, MauiTimePicker>
	{
		protected override MauiTimePicker CreatePlatformView()
		{
			return new MauiTimePicker(VirtualView);
		}

		internal bool UpdateImmediately
		{
			get => PlatformView.UpdateImmediately;
			set => PlatformView.UpdateImmediately = value;
		}

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
	}
#endif
}