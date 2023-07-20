using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.TimePicker)]
	public class TimePickerTests : ControlsHandlerTestBase
	{
		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler<TimePicker, TimePickerHandler>();
				});
			});
		}

		[Fact(DisplayName = "Does Not Leak")]
		public async Task DoesNotLeak()
		{
			SetupBuilder();
			WeakReference viewReference = null;
			WeakReference platformViewReference = null;
			WeakReference handlerReference = null;

			await InvokeOnMainThreadAsync(() =>
			{
				var layout = new Grid();
				var picker = new TimePicker();
				layout.Add(picker);
				var handler = CreateHandler<LayoutHandler>(layout);
				viewReference = new WeakReference(handler);
				handlerReference = new WeakReference(picker.Handler);
				platformViewReference = new WeakReference(picker.Handler.PlatformView);
			});

			await AssertionExtensions.WaitForGC(viewReference, handlerReference, platformViewReference);
			Assert.False(viewReference.IsAlive, "TimePicker should not be alive!");
			Assert.False(handlerReference.IsAlive, "Handler should not be alive!");
			Assert.False(platformViewReference.IsAlive, "PlatformView should not be alive!");
		}
	}
}
