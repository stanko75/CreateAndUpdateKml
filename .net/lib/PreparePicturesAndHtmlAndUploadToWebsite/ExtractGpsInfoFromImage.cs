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
                FileName = nameOfFileForJson,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
        }

        return null;
    }
}