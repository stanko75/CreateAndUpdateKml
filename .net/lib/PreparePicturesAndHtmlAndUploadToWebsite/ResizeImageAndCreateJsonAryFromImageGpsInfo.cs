namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class ResizeImageAndCreateJsonAryFromImageGpsInfo
{
    IResizeImage _resizeImage;
    IFillLatLngFileNameModelFromImageGpsInfo _fillLatLngFileNameModelFromImageGpsInfo;

    public ResizeImageAndCreateJsonAryFromImageGpsInfo(IResizeImage resizeImage, IFillLatLngFileNameModelFromImageGpsInfo fillLatLngFileNameModelFromImageGpsInfo)
    {
        _resizeImage = resizeImage;
        _fillLatLngFileNameModelFromImageGpsInfo = fillLatLngFileNameModelFromImageGpsInfo;
    }

    public LatLngFileNameModel? Execute(
        string originalFilename
        , string saveTo
        , int canvasWidth
        , int canvasHeight

        , string nameOfFileForJson)
    {
        _resizeImage.Execute(originalFilename
            , saveTo
            , canvasWidth
            , canvasHeight);

        return _fillLatLngFileNameModelFromImageGpsInfo.Execute(originalFilename, nameOfFileForJson);
    }
}