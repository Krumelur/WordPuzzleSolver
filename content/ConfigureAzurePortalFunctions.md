# Setup Azure Functions App

* In Azure Portal create a new resource using the type "Function App"
* In the dropdown where the storage account is configured, pick the previously created storage resource

## Configure configuration settings

Locally, our functions app uses the settings `local.settings.json`. This file is not under source control and contains the local configuration values, like the connection string to the Azure storage or storage emulator. 

To add the required connection strings and open the "Application settings" of the functions app and add the following entries entry to the "Application settings" section:

* `StorageConnectionString`. The value of this can be found in the storage account under Settings -> Access keys.
* `CustomVisionAIPredictionKey`: same as in the local configuration or copy from the AI portal (https://www.customvision.ai/)
* `CustomVisionAIEndpoint`: see instructions above.
* `CustomVisionAIProjectId`: see instructions above.