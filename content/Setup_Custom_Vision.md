# Setup Custom Vision

When a client uploads a word puzzle it arrives as one big image. We need to get access to the individual cells in that image that ultimately are characters. We have already create a helper method that splits an image into separate smaller parts, each containing one character. To process the characters we must convert them into ASCII values, so we can search for words. Recognizing image content is a job for the Azure Custom Vision AI.

> **Why are we not using Azure's prebuilt OCR (https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)?**
Azure has got a prebuild service to recognize text in images. This however cannot recognize individual characters. It needs context to understand the image content.

## Use Azure Portal to create a Custom Vision service

In the [Azure Portal](https://portal.azure.com) create a new "Custom Vision (preview)" resource.  Once the process has completed, there's really not a lot to do directly in the portal. Instead, go the "Overview" section of the just created resource and find the link "Custom Vision Portal". 

## Configure the Custom Vision project

In the portal of the custom vision service, sign in with the same account you used in Azure portal and create a new project using these settings:

* Name: anything you like
* Resource group: pick the one you created in the Azure portal
* Project types: classification
* Classification types: multiclass
* Domains: General

## Create training data

This is the tricky part. The goal is to recognize characters and support different fonts. Our word puzzles will only support uppercase characters to not make it too complicated. This means we will have the vision AI need to dfferentiate 26 images (A..Z).

I created a bash script that takes True Type Fonts (TTF) as input and turns every character into image. This is based on [ImageMagick](http://www.imagemagick.org). If you're intersted in the details, you can find the [script and the fonts I used in this repo](/utils/TTFConverter).

## Upload training data

The custom vision portal allows to batch upload multiple images. For every exported font, take the character images and upload them and assign tags to them (select all "A" and assign the tag "A" and so on). This is a pretty tedious process - I did it for seven font files and more might be needed to increase the recognition reliability. There's also a vision API available that could be used to automate the upload and tagging process. 

**At the time of this writing the link from the Azure portal to the API reference is pointing to an older (outdated?) version.**  The latest version seems to be 2.2 which can be found here: https://southcentralus.dev.cognitive.microsoft.com/docs/services/Custom_Vision_Training_2.2/operations/5b63704940d86a0fb87aab36

> Microsoft documentation even covers the [C# Nuget package](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/csharp-tutorial) available to communicate with Custom Vision API. I'll keep this as an exercise for the reader to automate the training process or maybe cover it in a different tutorial.

If all the images have been tagged, click the "Train" button in the top toolbar and let custom vision API do its magic. If you switch to the "Predictions" tab, you can try out the service by uploading a test image. It will also expose the restful endpoint to talk to the service.