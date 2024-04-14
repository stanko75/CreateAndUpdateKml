namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class CreateJsonAryFromImageGpsInfo: ICreateJsonAryFromImageGpsInfo
{
    private readonly IExtractGpsInfoFromImage _extractGpsInfoFromImage;
    private readonly IUpdateJsonIfExistsOrCreateNewIfNot _updateJsonIfExistsOrCreateNewIfNot;

    public CreateJsonAryFromImageGpsInfo(IExtractGpsInfoFromImage extractGpsInfoFromImage, IUpdateJsonIfExistsOrCreateNewIfNot updateJsonIfExistsOrCreateNewIfNot)
    {
        _extractGpsInfoFromImage = extractGpsInfoFromImage;
        _updateJsonIfExistsOrCreateNewIfNot = updateJsonIfExistsOrCreateNewIfNot;
    }

    public void Execute(string imageFileNameToReadGpsFrom, string nameOfFileForJson, string jsonFileName)
    {
        LatLngFileNameModel? latLngFileNameModel = _extractGpsInfoFromImage.Execute(imageFileNameToReadGpsFrom, nameOfFileForJson);
        if (latLngFileNameModel is null || latLngFileNameModel.lat == 0 || latLngFileNameModel.lng == 0)
        {
            throw new Exception("Cannot extract GPS info from image!");
        }
        else
        {
            _updateJsonIfExistsOrCreateNewIfNot.Execute(jsonFileName, latLngFileNameModel);
        }
    }
}