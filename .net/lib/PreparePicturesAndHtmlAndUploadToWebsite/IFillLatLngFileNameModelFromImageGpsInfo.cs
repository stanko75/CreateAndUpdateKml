namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IFillLatLngFileNameModelFromImageGpsInfo
{
    public LatLngFileNameModel? Execute(string imageFileNameToReadGpsFrom, string nameOfFileForJson);
}