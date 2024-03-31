namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class PrepareHmlFilesAndUpload
{
    private readonly IPrepareHtmlFiles _prepareHtmlFiles;
    private readonly IMirrorDirAndFileStructureOnFtp _mirrorDirAndFileStructureOnFtp;
    private readonly IWriteConfigurationToJsonFile _writeConfigurationToJsonFile;

    public PrepareHmlFilesAndUpload(IPrepareHtmlFiles prepareHtmlFiles
        , IMirrorDirAndFileStructureOnFtp mirrorDirAndFileStructureOnFtp
        , IWriteConfigurationToJsonFile writeConfigurationToJsonFile)
    {
        _prepareHtmlFiles = prepareHtmlFiles;
        _mirrorDirAndFileStructureOnFtp = mirrorDirAndFileStructureOnFtp;
        _writeConfigurationToJsonFile = writeConfigurationToJsonFile;
    }

    public void Execute(
        string htmlTemplateFolderWithRelativePath
        , string rootFolderWithRelativePathToCopy
        , string nameOfAlbum
        , string kmlFileName
       
        , string remoteRootFolder
        )
    {
        _prepareHtmlFiles.CopyHtmlTemplateForBlog(htmlTemplateFolderWithRelativePath, rootFolderWithRelativePathToCopy, nameOfAlbum, kmlFileName);
        _writeConfigurationToJsonFile.Execute($"https://www.milosev.com/gallery/allWithPics/travelBuddies/{nameOfAlbum}/"
            , kmlFileName
            , $@"{rootFolderWithRelativePathToCopy}\{nameOfAlbum}\config.json");
        _mirrorDirAndFileStructureOnFtp.Execute(Path.Join(rootFolderWithRelativePathToCopy, nameOfAlbum),
            $"{remoteRootFolder}/{nameOfAlbum}");

    }
}