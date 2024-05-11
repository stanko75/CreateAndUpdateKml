namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class FillLatLngFileNameModelFromImageGpsInfo: IFillLatLngFileNameModelFromImageGpsInfo
{
    private readonly IExtractGpsInfoFromImage _extractGpsInfoFromImage;

    public FillLatLngFileNameModelFromImageGpsInfo(IExtractGpsInfoFromImage extractGpsInfoFromImage)
    {
        _extractGpsInfoFromImage = extractGpsInfoFromImage;
    }

    public LatLngFileNameModel? Execute(string imageFileNameToReadGpsFrom, string nameOfFileForJson)
    {
        LatLngFileNameModel? latLngFileNameModel = _extractGpsInfoFromImage.Execute(imageFileNameToReadGpsFrom, nameOfFileForJson);
        if (latLngFileNameModel is null || latLngFileNameModel.Latitude == 0 || latLngFileNameModel.Longitude == 0)
        {
            throw new Exception("Cannot extract GPS info from image!");
        }

        return latLngFileNameModel;
    }
}