#nullable enable
using Microsoft.Maui;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Microsoft.Maui.Handlers.Benchmarks;

[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser]
public class PropertyMapperBenchmarker
{
	const int PropertyCount = 50;
	readonly Mock mock = new Mock();

	[Benchmark]
	public void UpdateProperties()
	{
		var mapper = new PropertyMapper<Mock, Mock>();
		for (int i = 0; i < PropertyCount; i++)
		{
			mapper.Add("Property" + i, (a, b) => { });
		}
		mapper.UpdateProperties(mock, mock);
	}

	class Mock : IElementHandler, IElement
	{
		public object? NativeView => null;

		public IElement? VirtualView => null;

		public IMauiContext? MauiContext => null;

		public IElementHandler? Handler { get; set; }

		public IElement? Parent => null;

		public void DisconnectHandler() { }

		public void Invoke(string command, object? args = null) { }

		public void SetMauiContext(IMauiContext mauiContext) { }

		public void SetVirtualView(IElement view) { }

		public void UpdateValue(string property) { }
	}
}