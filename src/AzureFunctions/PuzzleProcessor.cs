using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using PuzzleSolverLib;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

namespace AzureFunctions
{
    public static class PuzzleProcessor
    {
        [FunctionName("PuzzleProcessor")]
        public async static Task Run(
			[BlobTrigger("wordpuzzleuploads/{name}", Connection = "StorageConnectionString")]CloudBlockBlob blob,
			ILogger log)
        {
			log.LogInformation($"Image puzzle uploaded. Name:{blob.Name}");

			// Get BLOB content (the image). 
			var memoryStream = new MemoryStream();
			await blob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);
			memoryStream.Position = 0;

			// Split the image. Last element in the returned list will be the entire input image with the grid overlaid.
			IList<Stream> subImages;
			int numRows;
			int numColumns;
			var words = new List<string>();

			try
			{
				numRows = Convert.ToInt32(blob.Metadata["num_rows"]);
				numColumns = Convert.ToInt32(blob.Metadata["num_columns"]);
				var offsetLeft = Convert.ToInt32(blob.Metadata["offset_left"]);
				var offsetTop = Convert.ToInt32(blob.Metadata["offset_top"]);
				var spacingHorizontal = Convert.ToInt32(blob.Metadata["cell_spacing_horizontal"]);
				var spacingVertical = Convert.ToInt32(blob.Metadata["cell_spacing_vertical"]);
				var cellWidth = Convert.ToInt32(blob.Metadata["cell_width"]);
				var cellHeight = Convert.ToInt32(blob.Metadata["cell_height"]);
				words = blob.Metadata["words"].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(w => w.ToUpperInvariant().Trim()).ToList();
				
				subImages = ImageProcessing.SplitImage(
					memoryStream,
					numColumns,
					numRows,
					offsetLeft,
					offsetTop,
					spacingHorizontal,
					spacingVertical,
					cellWidth,
					cellHeight);
			}
			catch (Exception ex)
			{
				log.LogError($"Errror splitting image: {ex}");
				return;
			}

			// Let custom vision AI do its magic.
			// Note: when deploying this to Azure, don't forget to set these values in your app's config.
			var predictionKey = Environment.GetEnvironmentVariable("CustomVisionAIPredictionKey");
			var predictionEndpoint = Environment.GetEnvironmentVariable("CustomVisionAIEndpoint");
			var predictionProjectId = Environment.GetEnvironmentVariable("CustomVisionAIProjectId");

			log.LogInformation($"Predicting {numRows * numColumns} characters...");
			var predictionTasks = new List<Task<(double probability, char character)>> ();
			for(int row = 0; row < numRows; row++)
			{
				for(int col = 0; col < numColumns; col++)
				{
					int index = row * numColumns + col;
					predictionTasks.Add(ImageProcessing.PredictCharacterAsync(subImages[index], predictionKey, predictionEndpoint, predictionProjectId));
				}
			}
			await Task.WhenAll(predictionTasks).ConfigureAwait(false);

			// Search for words.
			var letterGrid = new char[numRows, numColumns];
			for (int row = 0; row < numRows; row++)
			{
				for (int col = 0; col < numColumns; col++)
				{
					var (probability, character) = predictionTasks[row * numColumns + col].Result;
					letterGrid[row, col] = character;
				}
			}

			// Dump out the grid of characters to logger.
			string dividerLine = new String('-', (letterGrid.GetUpperBound(1) + 1) * 2) + "-";
			log.LogInformation(dividerLine);

			for (int row = 0; row < letterGrid.GetUpperBound(0) + 1; row++)
			{
				var sb = new StringBuilder();
				for (int col = 0; col < letterGrid.GetUpperBound(1) + 1; col++)
				{
					sb.Append("|").Append(letterGrid[row, col]);
				}
				sb.Append("|");
				log.LogInformation(sb.ToString());
				log.LogInformation(dividerLine);
			}

			// Now do the search.
			foreach (var word in words)
			{
				SearchWord(letterGrid, word, log);
			}
        }

		static void SearchWord(char[,] letterGrid, string word, ILogger log)
		{
			log.LogInformation($"Searching for '{word}'...");
			var result = Solver.SearchForWord(letterGrid, word);
			if (result.Found)
			{
				log.LogInformation($"Found {result.Word} at {result.StartPos.Row}, {result.StartPos.Col}");
				foreach (Solver.LetterAtPosition item in result.Letters)
				{
					log.LogInformation($"  - '{item.Letter}' {item.Position.Row},{item.Position.Col}");
				}
			}
		}
	}
}
