using System.Text.Json;

namespace PreparePicturesAndHtmlAndUploadToWebsite;

//C:\projects\.net\console\CreateAndUpdateKmlConsoleDemo\.net\webApi\CreateAndUpdateKmlWebApi\CreateAndUpdateKmlWebApi\test.kml
//C:\projects\.net\console\CreateAndUpdateKmlConsoleDemo\.net\webApi\CreateAndUpdateKmlWebApi\CreateAndUpdateKmlWebApi\test.json
//C:\projects\.net\console\CreateAndUpdateKmlConsoleDemo\.net\webApi\CreateAndUpdateKmlWebApi\CreateAndUpdateKmlWebApi\config.json

//C:\projects\.net\console\CreateAndUpdateKmlConsoleDemo\html\blog
public class WriteConfigurationToJsonFile: IWriteConfigurationToJsonFile
{
    //string strAbsolutePath = "https://www.milosev.com";
    //string configFileName="config.json"
    public void Execute(string strAbsoluteWebPath, string kmlFileNameWithRelativeWebPath, string configFileName)
    {
        string jsonConfiguration = WriteConfigurationToJson(strAbsoluteWebPath, kmlFileNameWithRelativeWebPath);

        if (!string.IsNullOrEmpty(jsonConfiguration))
            File.WriteAllText(configFileName, jsonConfiguration);
    }

    private static string WriteConfigurationToJson(string strAbsolutePath, string kmlFileNameWithRelativePath)
    {
        Uri kmlUrl =
            Common.ConvertRelativeWindowsPathToUri(strAbsolutePath, Path.GetFileName(kmlFileNameWithRelativePath));
        var objConfig = new
        {
            KmlFileName = kmlUrl.AbsoluteUri
        };
        return JsonSerializer.Serialize(objConfig);
    }
}