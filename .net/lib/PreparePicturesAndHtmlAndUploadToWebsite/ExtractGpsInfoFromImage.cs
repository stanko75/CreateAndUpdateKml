using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class ExtractGpsInfoFromImage
{
    public LatLngFileNameModel? Execute(string fileName)
    {
        IReadOnlyList<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(fileName);
        GpsDirectory? gps = directories.OfType<GpsDirectory>().FirstOrDefault();
        GeoLocation? location = gps?.GetGeoLocation();
        if (location is not null)
        {
            return new LatLngFileNameModel
            {
                fileName = fileName, lat = location.Latitude, lng = location.Longitude
            };
        }

        return null;
    }
}