# Word Puzzle

I am a programmer. If you are reading this, chances are that you are also a programmer. I bet you will agree: if we see a problem, we begin to wonder if we should solve it manually or maybe write some code to solve the problem and all other related problems.

**Fun facts**

* We often don't care that solving the problem in a generic way takes way more time compared to the manual approach. 
* We love to totally overengineer things!

As part of his homework my son had to solve a word puzzle. A word puzzle is a grid of what appears to be random characters and he had to find the words hidden in there. Try to find the words "FUN", "FAKE" and "KITCHEN" in the grid below!

````
-----------------------------
| F | U | N | A | C | D | G |
-----------------------------
| A | R | J | K | O | M | T |
-----------------------------
| K | I | T | C | H | E | N |
-----------------------------
| E | X | N | L | Z | I | U |
-----------------------------
````

For the specific puzzle he was looking at he had found all words but one and he asked me to help. Instead of searching for the word, an idea started to form in my mind: let the computer find it! How hard can it be with the power of C#, Artifical Intelligence and Azure Cloud Computing! That's how this workshop was born.

## Target audience

* Azure developer

## Abstract

### Workshop
In this workshop, we will go through all the steps necessary to successfully create a FaaS (Function as a Service) based solution that allows uploading a picture of a word puzzle to the Azure cloud, setup an AI (Artificial Intelligence) to recognize the puzzle's characters and code an algorithm to find given words in the processed puzzle. Once everything is setup and deployed using Azure DevOps, we wil implement a Xamarin.Forms based client project to allow for direct upload of scanned puzzles.

After the workshop you will be able to create complex solutions by connecting different Azure based services. Each service by itself is simple and self contained.

### Application design overview

![Application Flow](assets/Flow.png)

1. Client acquires image
1. Client contacts Azure function to get upload information and receives upload URL as well as URL to retrieve results at
1. Client uploads puzzle image and meta data directly to Azure BLOB storage
1. BLOB storage triggers Azure function to kick off puzzle processing
1. Azure function contacts Azure Custom Vision API to extract text from uploaded image
1. Azure function runs code to find words in extracted text
1. Client polls URL to get results back

### Content

1. Setup Azure Storage Emulator
1. Create solution in Visual Studio
    1. Add and implement Puzzle solver project
    1. Add and implement Azure Functions project
        1. Receive a BLOB SAS
        1. Process an uploaded puzzle image
        1. Return processed puzzle to client
1. Configure services in Azure Portal
    1. BLOB storage
    1. Functions
    1. Custom Vision
        1. Setup
        1. Create and upload training data
1. Deploy solution to Azure
    1. Setup DevOps and Github integration
    2. Solve a word puzzle on Azure
1. Build a mobile client app