# Deploy solution to Azure

Deployment to the just created Function app can either be done directly from within Visual Studio by right clicking the functions project and selecting "Publish", from the command line using Azure CLI or with Azrue DevOps.

We'll go with the last option: DevOps.

## Deploy with Azure DevOps

The solution's code is already hosted on GitHub. We can use the repository to directly deploy from there. 

> **Note:** it is possible to configure automatic deployment directly from within Azure Portal. This is limited, however. It requires the repo to have a specific structure.

### Build the project

* Navigate to [Azure DevOps](https://dev.azure.com) and sign in using the same account used in Azure Portal. 
* Select an existing or create a new organization
* Create a project in the organization
* Add a new build pipeline
  * Connect to Github using OAuth if neccessary
  * Select the repository
  * Select the "C# Function" template
  * As path to the solution browse the repo and select the .csproj file of the Azure Function project
  * You can remove the step from the pipeline that runs the project tests because we don't have any tests (Yuck!)
  * Save & Queue the pipeline and make sure it builds

  > **Note:** I had to change the "Version of Nuget to install" from the preset "4.4.1" to ">=4.6" to correctly build the solution. You can find the version of Nuget running locally by checking Visual Studio -> Help/About

  To get continous integration, edit the build pipeline, select the tab "Triggers" and tick the checkbox "Enable continous integration".

### Deploy the project

TODO: Configuration on Azure! (Connection strings)



