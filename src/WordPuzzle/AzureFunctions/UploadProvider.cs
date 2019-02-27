using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Strg = Microsoft.WindowsAzure.Storage;

namespace AzureFunctions
{
	public static class UploadProvider
	{
		[FunctionName("UploadProvider")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
			[StorageAccount("StorageConnectionString")] Strg.CloudStorageAccount storage,
			ILogger log)
		{
			log.LogInformation("Upload requested");

			var blobClient = storage.CreateCloudBlobClient();
			Strg.Blob.CloudBlobContainer container = blobClient.GetContainerReference("wordpuzzleuploads");

			var containerExists = await container.ExistsAsync();
			if (!containerExists)
			{
				return new BadRequestObjectResult("Failed to find container.");
			}

			(string uri, string uniqueId) = GetContainerSaSUri(container);

			return new OkObjectResult(new {
				uploadurl = uri,
				id = uniqueId
			});
		}

		/// <summary>
		/// Creates a SAS for a BLOB and also returns a unique ID for later retrieval of the BLOB.
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		static (string uri, string uniqueId) GetContainerSaSUri(Strg.Blob.CloudBlobContainer container)
		{
			// Creating a SaS: https://docs.microsoft.com/en-us/azure/storage/blobs/storage-dotnet-shared-access-signature-part-2
			var sasConstraints = new Strg.Blob.SharedAccessBlobPolicy()
			{
				Permissions = Strg.Blob.SharedAccessBlobPermissions.Create | Strg.Blob.SharedAccessBlobPermissions.Write,
				SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(5)
			};

			var guid = Guid.NewGuid().ToString();
			var blob = container.GetBlobReference(guid);
			var url = blob.GetSharedAccessSignature(sasConstraints);

			return (uri: blob.Uri + url, uniqueId: guid);
		}
	}
}
