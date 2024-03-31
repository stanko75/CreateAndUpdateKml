namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IWriteConfigurationToJsonFile
{
    public void Execute(string strAbsoluteWebPath, string kmlFileNameWithRelativeWebPath, string configFileName);
}