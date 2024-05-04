namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class CopyHtmlFilesAndUpload(
    ICopyHtmlFiles copyHtmlFiles,
    IMirrorDirAndFileStructureOnFtp mirrorDirAndFileStructureOnFtp,
    IWriteConfigurationToJsonFile writeConfigurationToJsonFile) : ICopyHtmlFilesAndUpload
{
    public void Execute(
        string htmlTemplateFolderWithRelativePath
        , string rootFolderWithRelativePathToCopy
        , string nameOfAlbum
        , string kmlFileName
       
        , string remoteRootFolder
        )
    {
        copyHtmlFiles.CopyHtmlTemplateForBlog(htmlTemplateFolderWithRelativePath, rootFolderWithRelativePathToCopy, nameOfAlbum, kmlFileName);
        writeConfigurationToJsonFile.Execute($"https://www.milosev.com/gallery/allWithPics/travelBuddies/{nameOfAlbum}/"
            , kmlFileName
            , $@"{rootFolderWithRelativePathToCopy}\{nameOfAlbum}\config.json");
        mirrorDirAndFileStructureOnFtp.Execute(Path.Join(rootFolderWithRelativePathToCopy, nameOfAlbum),
            $"{remoteRootFolder}/{nameOfAlbum}");

    }
}