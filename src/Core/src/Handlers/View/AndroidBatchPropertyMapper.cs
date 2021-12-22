using System;
using System.Collections.Generic;

namespace Microsoft.Maui.Handlers
{
#if ANDROID
	class AndroidBatchPropertyMapper<TVirtualView, TViewHandler> : PropertyMapper<TVirtualView, TViewHandler>
		where TVirtualView : IElement
		where TViewHandler : IElementHandler
	{
		public PropertyBitMask PropertyMask { get; private set; }

		public AndroidBatchPropertyMapper(params IPropertyMapper[] chained) : base(chained) { }

		public AndroidBatchPropertyMapper<TVirtualView, TViewHandler> Initialized()
		{
			PropertyMask = PropertyBitMask.All;
			return this;
		}

		public override void Add(string key, Action<TViewHandler, TVirtualView> action)
		{
			if (PropertyMask != 0)
			{
				PropertyMask -= GetPropertyMask(key);
			}

			base.Add(key, action);
		}

		public override IEnumerable<string> GetKeys()
		{
			foreach (var key in _mapper.Keys)
			{
				// When reporting the key list for mass updates up the chain, ignore properties in the mask.
				// These will be handled by ViewHandler.SetVirtualView() instead.
				if ((PropertyMask & GetPropertyMask(key)) != 0)
				{
					continue;
				}

				yield return key;
			}

			if (Chained is not null)
			{
				foreach (var chain in Chained)
					foreach (var key in chain.GetKeys())
						yield return key;
			}
		}

		static PropertyBitMask GetPropertyMask(string name)
		{
			switch (name)
			{
				case nameof(IView.AutomationId):
					return PropertyBitMask.AutomationId;
				case nameof(IView.Visibility):
					return PropertyBitMask.Visibility;
				case nameof(IView.MinimumHeight):
					return PropertyBitMask.MinimumHeight;
				case nameof(IView.MinimumWidth):
					return PropertyBitMask.MinimumWidth;
				case nameof(IView.IsEnabled):
					return PropertyBitMask.IsEnabled;
				case nameof(IView.Opacity):
					return PropertyBitMask.Opacity;
				case nameof(IView.TranslationX):
					return PropertyBitMask.TranslationX;
				case nameof(IView.TranslationY):
					return PropertyBitMask.TranslationY;
				case nameof(IView.Scale):
					return PropertyBitMask.Scale;
				case nameof(IView.ScaleX):
					return PropertyBitMask.ScaleX;
				case nameof(IView.ScaleY):
					return PropertyBitMask.ScaleY;
				case nameof(IView.Rotation):
					return PropertyBitMask.Rotation;
				case nameof(IView.RotationX):
					return PropertyBitMask.RotationX;
				case nameof(IView.RotationY):
					return PropertyBitMask.RotationY;
				case nameof(IView.AnchorX):
					return PropertyBitMask.AnchorX;
				case nameof(IView.AnchorY):
					return PropertyBitMask.AnchorY;
				default:
					return PropertyBitMask.None;
			}
		}
	}

	enum PropertyBitMask : int
	{
		None          = 0,
		AutomationId  = 1 << 0,
		Visibility    = 1 << 1,
		FlowDirection = 1 << 2,
		MinimumHeight = 1 << 3,
		MinimumWidth  = 1 << 4,
		IsEnabled     = 1 << 5,
		Opacity       = 1 << 6,
		TranslationX  = 1 << 7,
		TranslationY  = 1 << 8,
		Scale         = 1 << 9,
		ScaleX        = 1 << 10,
		ScaleY        = 1 << 11,
		Rotation      = 1 << 12,
		RotationX     = 1 << 13,
		RotationY     = 1 << 14,
		AnchorX       = 1 << 15,
		AnchorY       = 1 << 16,
		All           = 0x1ffff, // This is all the values OR'd together
	}
#endif
}
