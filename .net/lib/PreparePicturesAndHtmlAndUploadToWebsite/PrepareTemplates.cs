using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class PrepareTemplates
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

        foreach (KeyValuePair<string, JToken?> keyValuesToReplaceInFiles in listOfKeyValuesToReplaceInFilesObject)
        {
            foreach (string fileToReplace in listOfFilesToReplace)
            {
                string fileToReplaceInFolder = Path.Join(templatesFolder, fileToReplace);
                if (File.Exists(fileToReplaceInFolder))
                {
                    string fileContent = File.ReadAllText(fileToReplaceInFolder);
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

                        string saveToFileNameWithPath = Path.Join(saveToPath, fileToReplace);
                        File.WriteAllText(saveToFileNameWithPath, fileContent);
                    }
                }
                else
                {
                    throw new Exception($"File: {fileToReplaceInFolder} not found!");
                }
            }
        }
    }
}