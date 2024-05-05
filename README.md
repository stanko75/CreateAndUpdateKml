# CreateAndUpdateKml
 Online GPS tracking from Android on Asp.Net

User starts live GPS tracking, and KML is saved in the folder "albumName", like "nameOfFile.kml", and at the same time file with last GPS position will be saved in "lastGPSposition.json", and file "config.json" will be also saved at the same time with following informations:

```
{
  "KmlFileName": "albumName/kmlFileName.kml",
  "CurrentLocation": "lastGPSposition.json",
  "LiveImageMarkersJsonUrl": "albumName/albumNameThumbs.json"
}
```

this configuration will be later used for HTML pages do display KML in google maps and image thumbs like icons on google maps

***

When user press button "Upload to blog", then in the folder "prepareForUpload" will be created following file structure:

```
"kml\kml.kml",
"pics",
"thumbs",
"www",
"www\css\index.css",
"www\lib\jquery-3.3.1.js",
"www\script\map.js",
"www\script\namespaces.js",
"www\script\pics2maps.js",
"www\script\thumbnails.js",
"www\index.html",
"www\joomlaPreview.html",
"www\albumName.js",
"www\albumNameThumbs.json"
```

***

When user press button "Upload pics", then the image will be uploaded to the server, GPS position will be extracted, and saved in albumName.js like:

```
[
    {
        "FileName": "../pics/imageName1.jpg",
        "Latitude": 50.4037590025,
        "Longitude": 7.25242328638889
    },
    {
        "FileName": "../pics/imageName2.jpg",
        "Latitude": 50.4027175902778,
        "Longitude": 7.25130748722222
    }
]
```

and in albumNameThumbs.json like:

```
[
    {
        "FileName": "../thumbs/imageName1.jpg",
        "Latitude": 50.4037590025,
        "Longitude": 7.25242328638889
    },
    {
        "FileName": "../thumbs/imageName2.jpg",
        "Latitude": 50.4027175902778,
        "Longitude": 7.25130748722222
    }
]
```
