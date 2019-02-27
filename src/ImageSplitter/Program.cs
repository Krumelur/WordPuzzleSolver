using PuzzleSolverLib;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ImageSplitter
{
	class Program
	{
		static void Main(string[] args)
		{
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("testpuzzle.jpg"));

			using (var imageStream = assembly.GetManifestResourceStream(resourceName))
			{
				const int numCols = 14;
				const int numRows = 14;

				var imgStreams = ImageProcessing.SplitImage(imageStream,
					columns: numCols,
					rows: numRows,
					offsetTop: 115,
					offsetLeft: 40,
					horizontalSpacing: 11,
					verticalSpacing: 12,
					cellWidth: 41,
					cellHeight: 40);

				int col = 0;
				int row = 0;

				foreach (var imgStream in imgStreams)
				{
					var fileStream = File.OpenWrite(imgStreams.IndexOf(imgStream) < imgStreams.Count - 1 ? $"{col}-{row}.png" : "overlay.png");
					imgStream.CopyTo(fileStream);
					fileStream.Close();

					col++;
					if (col >= numCols)
					{
						col = 0;
						row++;
					}
				}
			}
		}
	}
}
