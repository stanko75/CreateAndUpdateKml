namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IPrepareHtmlFiles
{
    public void CopyHtmlTemplateForBlog(
        string htmlTemplateFolderWithRelativePath
        , string rootFolderWithRelativePathToCopy
        , string nameOfAlbum
        , string kmlFileName);
}