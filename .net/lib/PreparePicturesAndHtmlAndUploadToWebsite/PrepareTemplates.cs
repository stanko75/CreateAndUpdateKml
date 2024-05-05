using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class PrepareTemplates : IPrepareTemplates
{
    public void ReplaceKeysInTemplateFilesWithProperValues(string listOfFilesToReplaceJson
        , string listOfKeyValuesToReplaceInFilesJson
        , string templatesFolder
        , string saveToPath)
    {
        List<string> listOfFilesToReplace = Common.LoadJsonFileAndConvertToList(listOfFilesToReplaceJson);
        JObject listOfKeyValuesToReplaceInFilesObject = Common.LoadJsonFileAndConvertToObject(listOfKeyValuesToReplaceInFilesJson);

        //string templatesFolder = @"C:\projects\.net\ConvertHtmlTemplates\template";

        //string saveToPath = "replacedTemplate\\test";

        foreach (string fileToReplace in listOfFilesToReplace)
        {
            string fileToReplaceInFolder = Path.Join(templatesFolder, fileToReplace);
            if (!File.Exists(fileToReplaceInFolder))
            {
                throw new Exception($"File: {Path.GetFullPath(fileToReplaceInFolder)} not found!");
            }

            string fileContent = File.ReadAllText(fileToReplaceInFolder);
            foreach (KeyValuePair<string, JToken?> keyValuesToReplaceInFiles in listOfKeyValuesToReplaceInFilesObject)
            {
                if (keyValuesToReplaceInFiles.Value is not null)
                {
                    fileContent = fileContent.Replace(keyValuesToReplaceInFiles.Key,
                        keyValuesToReplaceInFiles.Value.Value<string>());

                    if (!Directory.Exists(saveToPath))
                    {
                        Directory.CreateDirectory(saveToPath);
                    }

                    string fileToReplaceDir = Path.GetDirectoryName(fileToReplace) ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(fileToReplaceDir))
                    {
                        fileToReplaceDir = Path.Join(saveToPath, fileToReplaceDir);
                        if (!Directory.Exists(fileToReplaceDir))
                        {
                            Directory.CreateDirectory(fileToReplaceDir);
                        }
                    }

                }
            }
            string saveToFileNameWithPath = Path.Join(saveToPath, fileToReplace);
            File.WriteAllText(saveToFileNameWithPath, fileContent);
        }
    }
}