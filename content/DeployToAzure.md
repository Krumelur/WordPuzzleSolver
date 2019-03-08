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
  * Add another "Nuget restore" step and point it's source to the "PuzzleSolverLib" project. The functions project depends on it and Nuget will otherwise not restore the reference's packages. (_Alternatively using `dotnet restore` could work, it does not have the same limitations but I have not tried it_)
  * Save & Queue the pipeline and make sure it builds

  > **Note:** I had to change the "Version of Nuget to install" from the preset "4.4.1" to ">=4.6" to correctly build the solution. You can find the version of Nuget running locally by checking Visual Studio -> Help/About

  To get continous integration, edit the build pipeline, select the tab "Triggers" and tick the checkbox "Enable continous integration".

### Deploy the project

Switch to the "Releases" menu option and create a new pipeline.

* Select the template "Deploy a functiona pp to Azrue Functions"
* Select the just added stage (it should show a red exclamation mark and "1 job, 1 task") to configure it
  * Select the correct Azure subscription (and authorize if necessary)
  * Select the correct app service name from the dropdown
  * Save
* Switch back to the "Pipeline" tab
  * Click "Add" next to "Artifcats" 
  * Select "Build" as the source type
  * Select the correct DevOps project
  * Select the earlier created build pipeline as the source
  * Set the default version to "Latest"
  * Save
* Select the lightning icon in the Artifacts section and enable continous deployment whenever a new build is available.
* Save

### Test the CI/CD process

Make some changes in the local repo and push them to GitHub. It should trigger a build followed by a deployment.



