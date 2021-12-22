using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Xunit;
using AView = Android.Views.View;

namespace Microsoft.Maui.DeviceTests
{
	public partial class ViewHandlerTests
	{
		class MyHandler : ViewHandler<IView, AView>
		{
			static IPropertyMapper<IView, IViewHandler> Mapper = new AndroidBatchPropertyMapper<IView, IViewHandler>(ElementHandler.ElementMapper)
			{
				[nameof(IView.Opacity)] = (h, v) => SetAlpha(h, .9f),
			};

			static MyHandler() => Mapper.AppendToMapping(nameof(IView.Background), (h, v) => SetAlpha(h, .5f));

			public MyHandler() : base(Mapper) { }

			static void SetAlpha(IViewHandler h, float alpha) => ((AView)h.NativeView).Alpha = alpha;

			protected override AView CreateNativeView() => new AView(Application.Context);
		}

		[Fact(DisplayName = "Can override AndroidBatchPropertyMapper")]
		public void CanOverrideAndroidBatchPropertyMapper()
		{
			var handler = new MyHandler();
			var view = new StubBase();

			InitializeViewHandler(view, handler);
			var nativeView = (AView)handler.NativeView;
			nativeView.Initialize(PropertyBitMask.All, view);

			// Handler defaults to 0.9 alpha, then overrides to 0.5
			Assert.Equal(0.5f, nativeView.Alpha);
		}
	}
}