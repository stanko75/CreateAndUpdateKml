using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PreparePicturesAndHtmlAndUploadToWebsite;

static class Common
{
    public static Uri ConvertRelativeWindowsPathToUri(string strAbsolutePath, string relativeWindowsPath)
    {
        Uri uriAbsolute = new Uri(strAbsolutePath);
        return new Uri(uriAbsolute, relativeWindowsPath);
    }

    public static JObject LoadJsonFileAndConvertToObject(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new Exception($"File: {Path.GetFullPath(fileName)} does not exist!");
        }

        string jsonFileContent = File.ReadAllText(fileName);
        return JObject.Parse(jsonFileContent);
    }

    public static List<string> LoadJsonFileAndConvertToList(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new Exception($"File: {Path.GetFullPath(fileName)} does not exist!");
        }

        string jsonFileContent = File.ReadAllText(fileName);
        return JsonConvert.DeserializeObject<List<string>>(jsonFileContent);
    }
}