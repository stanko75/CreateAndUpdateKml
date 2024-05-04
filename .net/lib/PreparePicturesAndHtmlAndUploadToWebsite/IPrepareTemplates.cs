namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IPrepareTemplates
{
    public void ReplaceKeysInTemplateFilesWithProperValues(string listOfFilesToReplaceJson
        , string listOfKeyValuesToReplaceInFilesJson
        , string templatesFolder
        , string saveToPath);
}