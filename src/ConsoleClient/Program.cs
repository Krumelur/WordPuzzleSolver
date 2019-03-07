using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PuzzleSolverLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace ConsoleClient
{
	class Program
	{
		// Adjust to your configuration.
		public const string HOST_BASE_URL = "http://localhost:7071";

		const int NumRows = 14;
		const int NumColumns = 14;
		const int OffsetLeft = 40;
		const int OffsetTop = 115;
		const int CellWidth = 41;
		const int CellHeight = 40;
		const int VerticalSpacing = 12;
		const int HorizontalSpacing = 11;
		static string[] SearchWords = new string[] {"PERFUME", "HAIRSTYLE", "CURLING", "IRON", "MOUSTACHE",
			"BEARD", "EYESHADOW", "SAUNA", "STYLIST", "BRUSH", "PERMANENT", "SHAMPOO",
			"WAXING", "GOATEE", "BLOWDRYER", "MAKEUP", "TRIM", "RAZOR", "HAIRCUT", "COMB", "SPA"};

		static async Task Main(string[] args)
		{
			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("testpuzzle.jpg"));


			var httpClient = new HttpClient
			{
				BaseAddress = new Uri(HOST_BASE_URL)
			};

			using (var imageStream = assembly.GetManifestResourceStream(resourceName))
			{
				// Get upload information.
				var uploadInfo = JObject.Parse(await httpClient.GetStringAsync("api/uploadprovider").ConfigureAwait(false));
				var uploadUrl = uploadInfo["uploadurl"].ToString();
				var id = uploadInfo["id"].ToString();

				Console.WriteLine($"Upload ID is '{id}' - will upload to '{uploadUrl}'.");

				// Upload to BLOB storage.
				try
				{
					var blob = new CloudBlockBlob(new Uri(uploadUrl));
					blob.Metadata.Add("num_rows", NumRows.ToString());
					blob.Metadata.Add("num_columns", NumColumns.ToString());
					blob.Metadata.Add("offset_top", OffsetTop.ToString());
					blob.Metadata.Add("offset_left", OffsetLeft.ToString());
					blob.Metadata.Add("cell_spacing_horizontal", HorizontalSpacing.ToString());
					blob.Metadata.Add("cell_spacing_vertical", VerticalSpacing.ToString());
					blob.Metadata.Add("cell_width", CellWidth.ToString());
					blob.Metadata.Add("cell_height", CellHeight.ToString());
					blob.Metadata.Add("words", String.Join(',', SearchWords));

					await blob.UploadFromStreamAsync(imageStream).ConfigureAwait(false);
				}
				catch (StorageException ex)
				{
					Console.WriteLine($"*** Upload to BLOB storage failed: {ex.Message}");
				}

				// Wait for completion.
				HttpResponseMessage checkResponse;
				do
				{
					Console.WriteLine("Polling for puzzle completion...");
					checkResponse = await httpClient.GetAsync($"api/puzzleresults/{id}").ConfigureAwait(false);
					if (checkResponse.IsSuccessStatusCode)
					{
						var checkResult = JObject.Parse(await checkResponse.Content.ReadAsStringAsync().ConfigureAwait(false));
						Console.WriteLine($"Received results for BLOB {checkResult["id"]}");
						var searchResults = JsonConvert.DeserializeObject<List<Solver.SearchResult>>(checkResult["results"].ToString());
						foreach (var searchResult in searchResults)
						{
							Console.WriteLine(searchResult);
						}
					}
					else
					{
						await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
					}
				}
				while (!checkResponse.IsSuccessStatusCode);
			}

			Console.WriteLine();
			Console.WriteLine("Done.");
			Console.ReadKey();
		}
	}
}
