namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface ICreateJsonAryFromImageGpsInfo
{
    public void Execute(string imageFileNameToReadGpsFrom, string nameOfFileForJson, string jsonFileName);
}