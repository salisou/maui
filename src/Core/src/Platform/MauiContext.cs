using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Animations;

namespace Microsoft.Maui
{
	public class MauiContext : IMauiContext
	{
		readonly WrappedServiceProvider _services;
		readonly IMauiContext? _parent;

#if __ANDROID__
		public MauiContext(IServiceProvider services, Android.App.Application application, IMauiContext? parent = null)
			: this(services, (Android.Content.Context)application, parent)
		{
			AddSpecific(application);
		}

		public MauiContext(IServiceProvider services, Android.Content.Context context, IMauiContext? parent = null)
			: this(services, parent)
		{
			AddWeakSpecific(context);

			if (parent?.Services.GetService<NavigationRootManager>() == null && context is not Android.App.Application)
				AddSpecific(new NavigationRootManager(this));
		}
#elif __IOS__
		public MauiContext(IServiceProvider services, UIKit.UIApplicationDelegate application, IMauiContext? parent = null)
			: this(services, parent)
		{
			AddSpecific(application);
		}
#elif WINDOWS
		public MauiContext(IServiceProvider services, UI.Xaml.Application application, IMauiContext? parent = null)
			: this(services, parent)
		{
			AddSpecific(application);
		}
#endif

		public MauiContext(IMauiContext parent)
			: this(parent.Services, parent)
		{
		}

		public MauiContext(IServiceProvider services, IMauiContext? parent = null)
		{
			_services = new WrappedServiceProvider(services ?? throw new ArgumentNullException(nameof(services)));
			_parent = parent;

			// the animation manager should only be fetched once per context
			// TODO: maybe this should be set from outside?
			AddSpecific(() => services.GetRequiredService<IAnimationManager>());
		}

		public IServiceProvider Services => _services;

		public IMauiHandlersServiceProvider Handlers =>
			Services.GetRequiredService<IMauiHandlersServiceProvider>();

#if __ANDROID__
		public Android.Content.Context? Context =>
			Services.GetService<Android.Content.Context>();
#endif

		internal void AddSpecific<TService>(TService instance)
			where TService : class
		{
#if NETSTANDARD2_0
			var lazy = new Lazy<object?>(() => instance);
#else
			var lazy = new Lazy<object?>(instance);
#endif
			_services.AddSpecific(typeof(TService), lazy);
		}

		internal void AddWeakSpecific<TService>(TService instance)
			where TService : class
		{
			var weak = new WeakReference(instance);
			var lazy = new Lazy<object?>(() => weak.Target);

			_services.AddSpecific(typeof(TService), lazy);
		}

		internal void AddSpecific<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TService>(Func<TService> factory)
			where TService : class
		{
			var lazy = new Lazy<object?>(factory);

			_services.AddSpecific(typeof(TService), lazy);
		}

		internal void AddSpecific<TService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TImplementation>(Func<TImplementation> factory)
			where TService : class
			where TImplementation : class, TService
		{
			var lazy = new Lazy<object?>(factory);

			_services.AddSpecific(typeof(TService), lazy);
		}

		class WrappedServiceProvider : IServiceProvider
		{
			readonly IServiceProvider _serviceProvider;
			readonly ConcurrentDictionary<Type, Lazy<object?>> _scopeStatic = new();

			public WrappedServiceProvider(IServiceProvider serviceProvider)
			{
				_serviceProvider = serviceProvider;
			}

			public object? GetService(Type serviceType)
			{
				if (_scopeStatic.TryGetValue(serviceType, out var getter))
					return getter.Value;

				return _serviceProvider.GetService(serviceType);
			}

			public void AddSpecific(Type type, Lazy<object?> getter)
			{
				_scopeStatic[type] = getter;
			}
		}
	}
}