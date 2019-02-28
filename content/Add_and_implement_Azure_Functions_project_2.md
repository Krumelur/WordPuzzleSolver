# Create a BLOB triggered function

With the second function we're going to handle the processing of a previously uploaded image puzzle and call into the helper methods we added to the "PuzzleSolverLib" project.

## Puzzle processor

We need to create a function that gets triggered whenever a new upload has been added to the BLOB storage. The function will then read the uploaded puzzle image from the BLOB, split it into indivdual cells containing the characters and call a custom vision AI service to recognize the character. If all characters have been recognized, the function will call the puzzle solver library we created in the first part of this tutorial and store information about the found words in a database.

## Add a new function

Add a new function to the already existing "AzureFunctions" project in the solution and name it _PuzzleProcessor.cs_.

* Set the trigger to **Blob trigger**
* Use "StorageConnectionString" as the connection string setting
* Set path to "wordpuzzleuploads"

This will generate function that gets called whenever a new blob has been generated in the container we created previously. If you want to, you can try it out by running the functions project and then add a file using storage explorer. It should output the log message.

```cs
[FunctionName("PuzzleProcessor")]
public static void Run([BlobTrigger("wordpuzzleuploads/{name}", Connection = "StorageConnectionString")]Stream blob, string name, ILogger log)
```

The BLOB (our image) is conveniently passed to the function as an input parameter. However, there is room for improvement. BLOB triggers support various other input bindings and binding types. You can read more about it in the [Microsoft documentation](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-blob).

By changing the function's signature a bit we can get access the _actual_ BLOB and all of its related data:

```cs
public static void Run(
    [BlobTrigger("wordpuzzleuploads/{name}", Connection = "StorageConnectionString")]CloudBlockBlob blob,
    ILogger log)
```

Notice how instead of the stream we're now getting the BLOB itself. Pretty cool, isn't it? :-)

### Read meta information

When we implemented the helper method to split up in image into sub-images, we introduced the parameters required to perform this operation. When a BLOB is uploaded, the client can optionally provide meta data. This will be transferred in the header section of the request and then stored together with the BLOB. We will use this meta data to store the parameters necessary to split our word puzzle.

> **Note:** in the Azure Storage Explorer you can open any existing BLOB and add meta data manually. This is nice for testing purposes.

Let's define the names for the parameters:

* Number of columns: `num_cols` (14)
* Numnber of rows: `num_rows` (14)
* Top offset: `offset_top` (115)
* Left offset: `offset_left` (40)
* Width of the cells: `cell_width` (41)
* Height of the cells: `cell_height` (40)
* Vertical spacing between two rows: `cell_spacing_vertical` (12)
* Hortizontal spacing between to columns: `cell_spacing_horizontal` (11)

Using the Azure Storage Explorer, add an image of a word puzzle to the container we created earlier. You can use the [../assets/testpuzzle.jpg](test file in this repo). After the upload has completed, right click the BLOB and select _Properties -> Add Metadata_ and add the parameters shown above. If you used the puzzle image of the repo, set the values to what is shown above in parantheses.

To access metadata from within our C# function we can use the input bindings. The `blob` parameter has everything we need and cotains a `MetaData` property:

```cs
public IDictionary<string, string> Metadata { get; }
```

If you run the function and put a breakpoint in it, it will trigger for the BLOB you added in the Storage Explorer. You can then verify that all our metadata is indeed there.

### Split the image

The metadata provides us with all the information we need to split the image and we already have created our `ImageProcessing.SplitImage()` helper method. Let's add a reference to the "PuzzleSolverLib" project and call that method (Right click the "AzureFunction" project, then _Add -> Reference solution_) and call the `SplitImage()` method. Some parsing is required to convert the strings contained in the metadata dictionary into the correct data types. You can find details in the completed solution here in the repo. To get a stram from the BLOB, we can use

```cs
var memoryStream = new MemoryStream();
await blob.DownloadToStreamAsync(memoryStream).ConfigureAwait(false);
```

### Use custom vision service

https://southcentralus.dev.cognitive.microsoft.com/docs/services/450e4ba4d72542e889d93fd7b8e960de/operations/5a6264bc40d86a0ef8b2c290
