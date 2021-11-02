#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Maui.Hosting;

namespace Microsoft.Maui
{
	public interface IMauiHandlersServiceProvider : IMauiServiceProvider
	{
		Type? GetHandlerType([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type iview);

		IElementHandler? GetHandler([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type type);

		IElementHandler? GetHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>() where T : IElement;

		IMauiHandlersCollection GetCollection();
	}
}