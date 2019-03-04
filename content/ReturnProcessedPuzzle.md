# Return completed puzzle to client

As the final step in the puzzle solving process we need to offer the client a way to download the completed puzzle.

## Add a function to let the client query the puzzle status

We are going add another HTTP triggered function the client can call to check if the puzzle has been solved. In the completed project the function is called `PuzzleResults` and it reacts to an HTTP GET.

The function has a `StorageAccount` binding just like the `UploadProvider` function we created earlier. 
It also uses a route mapping the functions URL to `/puzzleresults/7c7d27d7-b251-4626-8331-c445160d9887`, where the guid is the one that was returned by the `UploadProvider`.

