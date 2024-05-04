namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface ICopyHtmlFilesAndUpload
{
    public void Execute(
        string htmlTemplateFolderWithRelativePath
        , string rootFolderWithRelativePathToCopy
        , string nameOfAlbum
        , string kmlFileName

        , string remoteRootFolder
    );
}