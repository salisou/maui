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
					handlers.AddHandler<Layout, LayoutHandler>();
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

			{
				var layout = new Grid();
				var picker = new TimePicker();
				layout.Add(picker);
				await CreateHandlerAndAddToWindow<LayoutHandler>(layout, handler =>
				{
					viewReference = new WeakReference(picker);
					handlerReference = new WeakReference(picker.Handler);
					platformViewReference = new WeakReference(picker.Handler.PlatformView);
				});
			}

			await AssertionExtensions.WaitForGC(viewReference, handlerReference, platformViewReference);
			Assert.False(viewReference.IsAlive, "TimePicker should not be alive!");
			Assert.False(handlerReference.IsAlive, "Handler should not be alive!");
			Assert.False(platformViewReference.IsAlive, "PlatformView should not be alive!");
		}
	}
}
