using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.IO;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.ImageSharp.PixelFormats;

namespace PuzzleSolverLib
{
	public static class ImageProcessing
	{
		/// <summary>
		/// Splits an input image into sub images of dimensions specified by the input parameters.
		/// </summary>
		/// <param name="imageStream">the image read</param>
		/// <param name="columns">number of columns</param>
		/// <param name="rows">number of rows</param>
		/// <param name="offsetLeft">left indentation</param>
		/// <param name="offsetTop">top indentation</param>
		/// <param name="horizontalSpacing">horizontal spacing between cells</param>
		/// <param name="verticalSpacing">vertical spacing between cells</param>
		/// <param name="cellWidth">width of a cell</param>
		/// <param name="cellHeight">height of a cell</param>
		/// <returns>
		/// A list of streams representing the sub images (one for each cell = rows * columns) row by row and one additional last entry
		/// containing the original image with the cell areas highlighted. All returned data is in PNG format.
		/// </returns>
		public static IList<Stream> SplitImage(
			Stream imageStream,
			int columns,
			int rows,
			int offsetLeft,
			int offsetTop,
			int horizontalSpacing,
			int verticalSpacing,
			int cellWidth,
			int cellHeight)
		{
			var imageStreams = new List<Stream>();

			using (var img = Image.Load(imageStream))
			using (var cellHighlightImg = img.Clone())
			{ 
				for (int currentRow = 0; currentRow < rows; currentRow++)
				{
					for (int currentCol = 0; currentCol < columns; currentCol++)
					{
						var x = offsetLeft + currentCol * (cellWidth + horizontalSpacing);
						var y = offsetTop + currentRow * (cellHeight + verticalSpacing);

						// Draw rectangles around the cells.
						cellHighlightImg.Mutate(ctx => ctx.DrawPolygon(Rgba32.Cyan, 1f,
							new PointF(x, y),
							new PointF(x + cellWidth - 1, y),
							new PointF(x + cellWidth - 1, y + cellHeight - 1),
							new PointF(x, y + cellHeight - 1)));

						// Save individual image of each cell.
						var croppedImg = img.Clone(input => input.Crop(new Rectangle(x, y, cellWidth, cellHeight)));
						var croppedStream = new MemoryStream();
						croppedImg.SaveAsPng(croppedStream);
						croppedStream.Position = 0;
						imageStreams.Add(croppedStream);
					}
				}

				var cellHighlightStream = new MemoryStream();
				cellHighlightImg.SaveAsPng(cellHighlightStream);
				cellHighlightStream.Position = 0;
				imageStreams.Add(cellHighlightStream);
			}

			return imageStreams;
		}
	}
}
