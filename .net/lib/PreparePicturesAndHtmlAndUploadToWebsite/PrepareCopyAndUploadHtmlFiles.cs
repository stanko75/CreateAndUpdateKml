namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class PrepareCopyAndUploadHtmlFiles(ICopyHtmlFilesAndUpload copyHtmlFilesAndUpload, IPrepareTemplates prepareTemplates)
{
    public void Execute(
        string listOfFilesToReplaceJson
        , string listOfKeyValuesToReplaceInFilesJson
        , string templatesFolder
        , string saveToPath

        , string htmlTemplateFolderWithRelativePath
        , string rootFolderWithRelativePathToCopy
        , string nameOfAlbum
        , string kmlFileName

        , string remoteRootFolder
    )
    {
        prepareTemplates.ReplaceKeysInTemplateFilesWithProperValues(listOfFilesToReplaceJson
            , listOfKeyValuesToReplaceInFilesJson
            , templatesFolder
            , saveToPath);

        copyHtmlFilesAndUpload.Execute(htmlTemplateFolderWithRelativePath
            , rootFolderWithRelativePathToCopy
            , nameOfAlbum
            , kmlFileName

            , remoteRootFolder);
    }
}