using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Platform;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.Image)]
	public partial class ImageTests : ControlsHandlerTestBase
	{
		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler<Image, ImageHandler>();
				});
			});
		}

		[Fact]
		public async Task ImageWithUndefinedSizeAndWithBackgroundSetRenders()
		{
			SetupBuilder();
			var layout = new VerticalStackLayout();

			var image = new Image
			{
				Background = Colors.Black,
				Source = "red.png",
			};

			layout.Add(image);

			await InvokeOnMainThreadAsync(async () =>
			{
				var handler = CreateHandler<LayoutHandler>(layout);
				await image.Wait();
				await handler.ToPlatform().AssertContainsColor(Colors.Red, MauiContext);
			});
		}

		[Fact("Image Does Not Leak")]
		public async Task DoesNotLeak()
		{
			SetupBuilder();
			WeakReference reference = null;

			await InvokeOnMainThreadAsync(async () =>
			{
				var layout = new VerticalStackLayout();
				var image = new Image
				{
					Background = Colors.Black,
					Source = "red.png",
				};
				layout.Add(image);

				var handler = CreateHandler<LayoutHandler>(layout);
				await image.Wait();
				reference = new WeakReference(image.Handler.PlatformView);
			});

			await Task.Yield();
			GC.Collect();
			GC.WaitForPendingFinalizers();

			Assert.NotNull(reference);
			Assert.False(reference.IsAlive, "PlatformView should not be alive!");
		}
	}
}