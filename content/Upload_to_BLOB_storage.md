# Upload to BLOB storage

When the client uploads the word puzzle to the (temprary) URL returned by the _UploadProvider_ function it must also include meta data about the word puzzle:

* The top and left offset of the puzzle within the image
* The width and height of each character cell
* The vertical and horizontal spacing between the cells
* The list of words to search

The above is necessary because we would otherwise need a more sophisticated image recognition software to elimate potential separator lines between the cells. We could get away without specifying the list of words to look for by adding a dictionary into our logic and let the service search for _any_ words. Maybe we'll get back to this in a later tutorial, but for iteration one, let's keep things a bit simpler.

## Providing meta data in the request header
