using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	[Category(TestCategory.RefreshView)]
	public partial class RefreshViewTests : ControlsHandlerTestBase
	{
		void SetupBuilder()
		{
			EnsureHandlerCreated(builder =>
			{
				builder.ConfigureMauiHandlers(handlers =>
				{
					handlers.AddHandler<Label, LabelHandler>();
					handlers.AddHandler<RefreshView, RefreshViewHandler>();
				});
			});
		}

		[Fact("Does Not Leak")]
		public async Task DoesNotLeak()
		{
			SetupBuilder();

			// Long-lived ICommand, like a Singleton ViewModel
			var command = new MyCommand();
			WeakReference reference = null;

			await InvokeOnMainThreadAsync(() =>
			{
				var layout = new Grid();
				var refreshView = new RefreshView
				{
					Command = command,
				};
				var label = new Label();
				refreshView.Content = label;
				layout.Add(refreshView);

				var handler = CreateHandler<LayoutHandler>(layout);
				reference = new(refreshView);
			});

			Assert.NotNull(reference);

			// Several GCs required on iOS
			await AssertionExtensions.Wait(() =>
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				return !reference.IsAlive;
			}, timeout: 5000);

			Assert.False(reference.IsAlive, "RefreshView should not be alive!");
		}

		class MyCommand : ICommand
		{
			public event EventHandler CanExecuteChanged;

			public bool CanExecute(object parameter) => true;

			public void Execute(object parameter) { }

			public void FireCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
