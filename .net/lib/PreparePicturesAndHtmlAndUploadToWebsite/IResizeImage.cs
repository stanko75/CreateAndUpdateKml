namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IResizeImage
{
    public void Execute(string originalFilename
        , string saveTo
        , int canvasWidth
        , int canvasHeight);
}