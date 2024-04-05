namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class ResizeImageAndCreateJsonAryFromImageGpsInfo
{
    IResizeImage _resizeImage;
    ICreateJsonAryFromImageGpsInfo _createJsonAryFromImageGpsInfo;

    public ResizeImageAndCreateJsonAryFromImageGpsInfo(IResizeImage resizeImage, ICreateJsonAryFromImageGpsInfo createJsonAryFromImageGpsInfo)
    {
        _resizeImage = resizeImage;
        _createJsonAryFromImageGpsInfo = createJsonAryFromImageGpsInfo;
    }

    public void Execute(
        string originalFilename
        , string saveTo
        , int canvasWidth
        , int canvasHeight

        , string nameOfFileForJson
        , string jsonFileName)
    {
        _resizeImage.Execute(originalFilename
            , saveTo
            , canvasWidth
            , canvasHeight);

        _createJsonAryFromImageGpsInfo.Execute(originalFilename, nameOfFileForJson, jsonFileName);
    }
}