# Setup Custom Vision

When a client uploads a word puzzle it arrives as one big image. We need to get access to the individual cells in that image that ultimately are characters. We'll later see how we can use an Azure function to split the image into separate smaller parts, each containing one character. To process the characters we must convert them into ASCII values, so we can search for words. Recognizing image content is a job for the Azure Custom Vision AI.

> **Why are we not using Azure's prebuilt OCR (https://azure.microsoft.com/en-us/services/cognitive-services/computer-vision/)?**
> 
>  Azure has got a prebuild service to recognize text in images. This however cannot recognize individual characters. It needs context to understand the image content.

## Use Azure Portal to create a Custom Vision service

In the [Azure Portal](https://portal.azure.com) create a new "Custom Vision (preview)" resource.  Once the process has completed, there's really not a lot to do directly in the portal. Instead, go the "Overview" section of the just created resource and find the link "Custom Vision Portal". 

## Configure the Custom Vision project

In the portal of the custom vision service, sign in with the same account you used in Azure portal and create a new project using these settings:

* Name: anything you like
* Resource group: pick the one you created in the Azure portal
* Project types: classification
* Classification types: multiclass
* Domains: General

## Upload training data

This is the tricky part. The goal is to recognize characters and support different fonts. Our word puzzles will only support uppercase characters to not make it too complicated. This means we will have the vision AI need to dfferentiate 26 images (A..Z).

I created a bash script that takes True Type Fonts (TTF) as input and turns every character into image. This is based on [ImageMagick](http://www.imagemagick.org). If you're intersted in the details, you can find the [script and the fonts I used in this repo]().