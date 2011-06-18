using System;
using System.Drawing;

namespace Domain
{
	public class StoredImage
	{
		public bool IsPrimary { get; internal set; }
		public readonly Size ThumbnailSize = new Size(100, 100);

		public StoredImage()
		{
		}

		// This will only account for the greater of the dimensions.. You don't want to smoosh the image
		//  thanks to: http://www.switchonthecode.com/tutorials/csharp-tutorial-image-editing-saving-cropping-and-resizing
		private System.Drawing.Image GetResampledImage(System.Drawing.Image toResize, Size desiredSize)
		{
			if (toResize == null) throw new ArgumentNullException("toResize");

			double scaleFactor;

			// If the image is wider than it is tall, scale by the width
			if (toResize.Width > toResize.Height)
			{
				scaleFactor = desiredSize.Width / toResize.Width;
			}
			else // scale by height
			{
				scaleFactor = desiredSize.Height / toResize.Height;
			}
			var newSize = new Size((int)(toResize.Width * scaleFactor), (int)(toResize.Height * scaleFactor));

			var b = new Bitmap(newSize.Width, newSize.Height);
			var g = Graphics.FromImage(b);
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

			g.DrawImage(toResize, new Rectangle(new Point(0, 0), newSize));

			return (System.Drawing.Image)b;
		}
	}
}