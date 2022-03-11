#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics.Drawables;
using Bumptech.Glide;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.BumptechGlide;

namespace Microsoft.Maui
{
	public class ResourceOrDrawable
	{
		readonly Func<int, Drawable>? func;
		Drawable? drawable;

		public static implicit operator ResourceOrDrawable(Drawable drawable) => new ResourceOrDrawable(drawable);

		public ResourceOrDrawable(int resourceId, Func<int, Drawable> func)
		{
			ResourceId = resourceId;
			this.func = func;
		}

		public ResourceOrDrawable(Drawable drawable) => this.drawable = drawable;

		public int? ResourceId { get; private set; }

		public Drawable? Drawable => drawable ??= ResourceId != null ? func?.Invoke(ResourceId.Value) : null);
	}

	public partial class FileImageSourceService
	{
		public override Task<IImageSourceServiceResult<ResourceOrDrawable>?> GetDrawableAsync(IImageSource imageSource, Context context, CancellationToken cancellationToken = default) =>
			GetDrawableAsync((IFileImageSource)imageSource, context, cancellationToken);

		public async Task<IImageSourceServiceResult<ResourceOrDrawable>?> GetDrawableAsync(IFileImageSource imageSource, Context context, CancellationToken cancellationToken = default)
		{
			if (imageSource.IsEmpty)
				return null;

			var filename = imageSource.File;

			try
			{
				ImageSourceServiceResult? result = null;
				var id = context.GetDrawableId(filename);
				if (id > 0)
				{
					var drawable = new ResourceOrDrawable(id, context.GetDrawable);
					result = new ImageSourceServiceResult(drawable);
				}

				if (result == null)
				{
					result = await Glide
						.With(context)
						.Load(filename, context)
						.SubmitAsync(context, cancellationToken)
						.ConfigureAwait(false);
				}

				if (result == null)
					throw new InvalidOperationException("Unable to load image file.");

				return result;
			}
			catch (Exception ex)
			{
				Logger?.LogWarning(ex, "Unable to load image file '{File}'.", filename);
				throw;
			}
		}
	}
}