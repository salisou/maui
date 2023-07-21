﻿using System;
using Android.Content;
using AndroidX.AppCompat.Widget;
using MemoryAnalyzers.Attributes;

namespace Microsoft.Maui.Platform
{
	public class MauiTextView : AppCompatTextView
	{
		public MauiTextView(Context context) : base(context)
		{
		}

		[MemoryLeakSafe("Android-only type. MauiTextView should have the same lifetime as subscribers: Label.")]
		internal event EventHandler<LayoutChangedEventArgs>? LayoutChanged;

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			LayoutChanged?.Invoke(this, new LayoutChangedEventArgs(l, t, r, b));
		}
	}

	public class LayoutChangedEventArgs : EventArgs
	{
		public LayoutChangedEventArgs()
		{

		}

		public LayoutChangedEventArgs(int l, int t, int r, int b)
		{
			Left = l;
			Top = t;
			Right = r;
			Bottom = b;
		}

		public int Left { get; set; }
		public int Top { get; set; }
		public int Right { get; set; }
		public int Bottom { get; set; }
	}
}
