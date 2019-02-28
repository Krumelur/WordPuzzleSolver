using PuzzleSolverLib;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomVisionTest
{
	class Program
	{
		static void Main(string[] args)
		{
			// Reading config values for custom vision service from a file called "Local.App.config" which is not under
			// source control so I don't leak my config :-)
			// There's a Template.Local.App.config file included you can use as a - well - template.
			var configMap = new ExeConfigurationFileMap
			{
				ExeConfigFilename = @"Local.App.config"
			};
			var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

			var predictionKey = config.AppSettings.Settings["CustomAIPredictionKey"]?.Value;
			var endpoint = config.AppSettings.Settings["Endpoint"]?.Value;
			var projectId = config.AppSettings.Settings["ProjectId"]?.Value;

			var assembly = Assembly.GetExecutingAssembly();
			var resourceNames = assembly.GetManifestResourceNames().Where(x => x.EndsWith(".png"));

			foreach (var resourceName in resourceNames)
			{
				using (var imageStream = assembly.GetManifestResourceStream(resourceName))
				{
					var (probability, character) = ImageProcessing.PredictCharacterAsync(imageStream, predictionKey, endpoint, projectId).Result;
					Console.WriteLine($"Prediction for {resourceName}: {probability:P1} sure this is a {character}");
				}
			}

			Console.WriteLine("Done.");
			Console.ReadKey();
		}
	}
}
