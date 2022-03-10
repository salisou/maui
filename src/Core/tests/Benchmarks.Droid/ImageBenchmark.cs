using Android.Graphics.Drawables;
using Bumptech.Glide;
using Bumptech.Glide.Request.Target;
using Bumptech.Glide.Request.Transition;
using AImageView = Android.Widget.ImageView;

namespace Benchmarks.Droid;

// SimpleTarget is apparently obsolete?
#pragma warning disable CS0612

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class ImageBenchmark
{
	MyTarget? target;
	AImageView? imageView;

	[GlobalSetup]
	public void GlobalSetup () => imageView = new AImageView(Application.Context);

	[GlobalCleanup]
	public void GlobalCleanup()
	{
		target?.Dispose();
		target = null;

		imageView?.Dispose();
		imageView = null;
	}

	[Benchmark]
	public void SetImageResource() => imageView!.SetImageResource(Resource.Drawable.dotnet_bot);

	[Benchmark]
	public void SetImageDrawable() => imageView!.SetImageDrawable(Application.Context.GetDrawable(Resource.Drawable.dotnet_bot));

	[Benchmark]
	public void GlideWithTarget() => Glide.With(imageView).Load(Resource.Drawable.dotnet_bot).Into(target = new MyTarget (imageView!));

	[Obsolete]
	class MyTarget : SimpleTarget
	{
		readonly ImageView imageView;

		public MyTarget(ImageView imageView)
		{
			this.imageView = imageView;
		}

		public override void OnResourceReady(Java.Lang.Object resource, ITransition transition)
		{
			if (resource is BitmapDrawable bitmap)
			{
				imageView.SetImageDrawable(bitmap);
			}
		}
	}
}
