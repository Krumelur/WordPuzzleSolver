# Send image to custom vision AI

As part of the processing we will have to send the individual characters of the puzzle to the AI and get back the corresponding ASCII values.

Just like with the word search and the image splitter, we are going to add a helper method to the "PuzzleSolverLib" project which we will then call from the Azure function.

As always, details are covered in the [Microsoft documentation](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/csharp-tutorial-od).

> **Note:** we are using the .Net client SDK to communicate with custom vision. There is also a [restful API](https://southcentralus.dev.cognitive.microsoft.com/docs/services/450e4ba4d72542e889d93fd7b8e960de/operations/5a6264bc40d86a0ef8b2c290
) available.

## Communicate with the AI

Start by adding a new method to class `ImageProcessing` in the "PuzzleSolverLib" project and create a `CustomVisionPredictionClient` instance as shown in the documentation referenced above. You can find the requried keys and endpoints in the custom vision project we created earlier.

```cs
public async static Task<(double probability, char character)> PredictCharacterAsync(
    Stream imageStream,
    string predictionKey,
    string endpoint,
    string projectId)
```

> **Note:** don't forget to add the `Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction` Nuget package.

You can find the completed version of this method in the repo.