using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class ExtractGpsInfoFromImage : IExtractGpsInfoFromImage
{
    public LatLngFileNameModel? Execute(string imageFileNameToReadGpsFrom, string nameOfFileForJson)
    {
        IReadOnlyList<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(imageFileNameToReadGpsFrom);
        GpsDirectory? gps = directories.OfType<GpsDirectory>().FirstOrDefault();
        GeoLocation? location = gps?.GetGeoLocation();
        if (location is not null)
        {
            return new LatLngFileNameModel
            {
                fileName = nameOfFileForJson,
                lat = location.Latitude,
                lng = location.Longitude
            };
        }

        return null;
    }
}