namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IExtractGpsInfoFromImage
{
    public LatLngFileNameModel? Execute(string imageFileNameToReadGpsFrom, string nameOfFileForJson);
}