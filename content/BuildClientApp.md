# Build a client app

To test the infrastructure we have built, let's create a simple client app. This will be console based. The concepts introduced also apply to a mobile client app likewise.

## Add the project

If you like to organize your solution like I do, add a solution folder called "Client" and create a new console based .NET Core application in it.

### The client's tasks

To completely test the flow of our infrastructure the client is going to perform the following things:

* Send an HTTP GET request to upload provider function to retrieve a URL to upload to
* Upload a puzzle image to the returned URL
* Periodically check the if the puzzle processing has been completed by issuing an HTTP GET against the Azure function that returns the results
* Print the results to the console

### Implementation

As always, the repo contains the completed project's source code to check. There are certain things noteworthy:

* The client project uses an async main method (`static async Task Main(string[] args)`). This is possible after setting the C# language version to 7.1 or above. The default setting of "Latest major version" will make it 7.0 which does not support async main methods.
* It would be possible to communicate with BLOB storage using a plain vanially `HttpClient` but the client is using the C# client storage library `Microsoft.Azure.Storage.Blob`.
* The client app is referencing the PuzzleSolverLib project to deserialize the server's response.


> **Note:** Because the client needs to communicate with Azure functions which we are also running locally, the `AzureFunctions` project must be built and run first and then the client must be started. Either right click the solution node and select "Set StartUp projects..." and configure the starup order and projects, or open a second VS instance and run AzureFunctions in one and the client in the other. A third option is ro right click the AzureFunctions project and select "Start new instance" and then do the same for the client project.