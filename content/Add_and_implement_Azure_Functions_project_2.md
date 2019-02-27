# Add and implement Azure Functions project

With the second function we're going to handle the processing of a previously uploaded image puzzle.

## Puzzle processor

We need to create a function that gets triggered whenever a new upload has been added to the BLOB storage. The function will then load the uploaded puzzle image from the BLOB, split it into indivdual cells containing the characters and call a custom vision AI service to recognize the character. If all characters have been recognized, the function will call the puzzle solver library we created in the first part of this tutorial and store information about the found words in a database.

## Add a new function

Add a new function to the already existing "AzureFunctions" project in the solution and name it _PuzzleProcessor.cs_.

* Set the trigger to **Blob trigger**
* Use "StorageConnectionString" as the connection string setting
* Set path to "wordpuzzleuploads"

This will generate function that gets called whenever a new blob has been generated in the container we created previously. If you want to, you can try it out by running the functions project and then add a file using storage explorer. It should output the log message.

### Read meta information

### Split the image

To split the word puzzle into separate sub images representing the individual characters we're going to use a libary called [ImageSharp](https://github.com/SixLabors/ImageSharp). The code will be called by the function but should really go into a reusable component, so we will add it to the **PuzzleSolverLib** we created earlier.

* Add the Nuget package `SixLabors.ImageSharp` to the PuzzleSolverLib project (make sure to search for prereleases too!)
* Add a new static class to the project called `ImageProcessing.cs`



### Use custom vision service

https://southcentralus.dev.cognitive.microsoft.com/docs/services/450e4ba4d72542e889d93fd7b8e960de/operations/5a6264bc40d86a0ef8b2c290
