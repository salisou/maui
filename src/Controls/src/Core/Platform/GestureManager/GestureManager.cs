#nullable enable

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.Platform
{
	internal class GestureManager
	{
		readonly IControlsView _view;
		WeakReference? _containerView;
		WeakReference? _platformView;
		WeakReference? _handler;

		public bool IsConnected => GesturePlatformManager != null;
		public GesturePlatformManager? GesturePlatformManager { get; private set; }

		public GestureManager(IControlsView view)
		{
			_view = view;
			view.HandlerChanging += OnHandlerChanging;
			view.HandlerChanged += OnHandlerChanged;
			view.WindowChanged += OnWindowChanged;
			view.PlatformContainerViewChanged += OnPlatformContainerViewChanged;

			SetupGestureManager();
		}

		void OnPlatformContainerViewChanged(object? sender, EventArgs e) =>
			SetupGestureManager();

		void OnWindowChanged(object? sender, EventArgs e) =>
			SetupGestureManager();

		void OnHandlerChanged(object? sender, EventArgs e) =>
			SetupGestureManager();

		void OnHandlerChanging(object? sender, HandlerChangingEventArgs e) =>
			DisconnectGestures();

		void DisconnectGestures()
		{
			GesturePlatformManager?.Dispose();
			GesturePlatformManager = null;
			_handler = null;
			_containerView = null;
			_platformView = null;
		}

		void SetupGestureManager()
		{
			var handler = _view.Handler;
			var window = _view.Window;

			if (handler == null ||
				window == null)
			{
				DisconnectGestures();
				return;
			}

			if (_containerView?.Target != handler.ContainerView ||
				_platformView?.Target != handler.PlatformView ||
				_handler?.Target != handler)
			{
				DisconnectGestures();
			}

			// The connected Gesture Manager is already setup and watching the correct view
			if (GesturePlatformManager != null)
				return;

			GesturePlatformManager = new GesturePlatformManager(handler);
			_handler = new WeakReference(handler);
			_containerView = new WeakReference(handler.ContainerView);
			_platformView = new WeakReference(handler.PlatformView);
		}
	}
}
