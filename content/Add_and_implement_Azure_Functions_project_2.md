# Add and implement Azure Functions project

With the second function we're going to handle the processing of a previously uploaded image puzzle.

## Puzzle processor

We need to create a function that gets triggered whenever a new upload has been added to the BLOB storage. The function will then load the uploaded puzzle image from the BLOB, split it into indivdual cells containing the characters and call a custom vision AI service to recognize the character. If all characters have been recognized, the function will call the puzzle solver library we created in the first part of this tutorial and store information about the found words in a database.