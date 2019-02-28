using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using PuzzleSolverLib;

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

			var memoryStream = new MemoryStream();
			await blob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);

			IList<Stream> subImages = ImageProcessing.SplitImage(
				imageStream: memoryStream,
				columns: Convert.ToInt32(blob.Metadata["num_columns"]),
				rows: Convert.ToInt32(blob.Metadata["num_rows"]),
				offsetLeft: Convert.ToInt32(blob.Metadata["offset_left"]),
				offsetTop: Convert.ToInt32(blob.Metadata["offset_top"]),
				horizontalSpacing: Convert.ToInt32(blob.Metadata["spacing_horizontal"]),
				verticalSpacing: Convert.ToInt32(blob.Metadata["spacing_vertical"]),
				cellWidth: Convert.ToInt32(blob.Metadata["cell_width"]),
				cellHeight: Convert.ToInt32(blob.Metadata["cell_height"]));
        }
    }
}
