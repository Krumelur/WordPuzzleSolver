using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Strg = Microsoft.WindowsAzure.Storage;

namespace AzureFunctions
{
	public static class GetPuzzleResults
    {
        [FunctionName("PuzzleResults")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "puzzleresults/{blobGuid}")] HttpRequest req,
			string blobGuid,
			[StorageAccount("StorageConnectionString")] Strg.CloudStorageAccount storage,
			ILogger log)
        {
            log.LogInformation($"Get puzzle results for id = {blobGuid}");

			var blobClient = storage.CreateCloudBlobClient();
			Strg.Blob.CloudBlobContainer container = blobClient.GetContainerReference("wordpuzzleuploads");

			var containerExists = await container.ExistsAsync().ConfigureAwait(false);
			if (!containerExists)
			{
				return new BadRequestObjectResult("Failed to find container.");
			}

			var blob = container.GetBlockBlobReference(blobGuid.ToString());
			bool blobExists = await blob.ExistsAsync().ConfigureAwait(false);
			if (!blobExists)
			{
				return new NotFoundObjectResult(new
				{
					id = blobGuid,
					description = "Cannot find an upload matching the ID.",
				});
			}

			await blob.FetchAttributesAsync().ConfigureAwait(false);

			if (!blob.Metadata.ContainsKey("results"))
			{
				return new NotFoundObjectResult(new
				{
					id = blobGuid,
					description = "The puzzle has not been processed. Please retry later.",
				});
			}

			// BLOB was found and contains result data.
			return new OkObjectResult(new {
				id = blobGuid,
				results = blob.Metadata["results"],
			});
		}
    }
}
