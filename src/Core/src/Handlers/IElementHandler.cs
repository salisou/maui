namespace Microsoft.Maui
{
	public interface IElementHandler
	{
		void SetMauiContext(IMauiContext mauiContext);

		void SetVirtualView(IElement view);

		void UpdateValue(string property);

		void Invoke(string command, object? args = null);

		void DisconnectHandler();

		void OnWindowChanged(object? oldValue, object? newValue);

		object? PlatformView { get; }

		IElement? VirtualView { get; }

		IMauiContext? MauiContext { get; }
	}
}