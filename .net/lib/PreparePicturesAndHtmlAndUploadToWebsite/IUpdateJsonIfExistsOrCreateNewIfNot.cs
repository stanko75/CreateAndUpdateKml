namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IUpdateJsonIfExistsOrCreateNewIfNot
{
    public void Execute(string jsonFileName, LatLngFileNameModel latLngFileNameModel);
}