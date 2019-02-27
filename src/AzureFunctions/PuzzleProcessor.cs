using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public static class PuzzleProcessor
    {
        [FunctionName("PuzzleProcessor")]
        public static void Run([BlobTrigger("wordpuzzleuploads/{name}", Connection = "StorageConnectionString")]Stream blob, string name, ILogger log)
        {
            log.LogInformation($"Image puzzle uploaded. Name:{name}, Size: {blob.Length} Bytes");
		
        }
    }
}
