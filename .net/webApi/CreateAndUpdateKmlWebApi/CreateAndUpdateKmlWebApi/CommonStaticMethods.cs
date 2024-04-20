using CreateAndUpdateKmlWebApi.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CreateAndUpdateKmlWebApi;

public static class CommonStaticMethods
{
    public static string CreateFolderIfNotExistAndChangeFileExtenstion(string folder, string fileName, string extension)
    {
        if (!string.IsNullOrWhiteSpace(folder) && !Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        fileName = ChangeFileExtension(fileName, extension);

        return Path.Combine(folder, fileName);
    }

    public static string ChangeFileExtension(string fileName, string extension)
    {
        if (!Path.GetExtension(fileName).Equals(extension, StringComparison.OrdinalIgnoreCase))
        {
            fileName = Path.ChangeExtension(fileName, extension);
        }

        return fileName;
    }

    public static void WriteConfigurationToJsonFile(string folderName
        , string kmlFileName
        , string currentLocation
        , string configFileName
        , string rootUrl)
    {
        LiveConfigModel liveExistingConfigModel;
        if (!rootUrl.EndsWith("/"))
        {
            rootUrl += "/";
        }
        //if (File.Exists(configFileName))
        {
            string strExistingConfig = File.ReadAllText(configFileName);
            JObject jsonObjectExistingConfig = JObject.Parse(strExistingConfig);
            liveExistingConfigModel = jsonObjectExistingConfig.ToObject<LiveConfigModel>() ??
                                                      throw new InvalidOperationException();

            if (!string.IsNullOrWhiteSpace(folderName))
            {
                folderName =
                    ConvertRelativeWindowsPathToUri(rootUrl, folderName).AbsoluteUri;
                folderName += "/";

                if (!string.IsNullOrWhiteSpace(kmlFileName))
                {
                    liveExistingConfigModel.LiveImageMarkersJsonUrl =
                        ConvertRelativeWindowsPathToUri($"{folderName}",
                                $"{Path.GetFileNameWithoutExtension(kmlFileName)}Thumbs.json")
                            .AbsoluteUri;

                    liveExistingConfigModel.KmlFileName =
                        ConvertRelativeWindowsPathToUri(folderName, kmlFileName)
                            .AbsoluteUri;
                }

            }

            if (!string.IsNullOrWhiteSpace(currentLocation))
            {
                liveExistingConfigModel.CurrentLocation = ConvertRelativeWindowsPathToUri(rootUrl,
                        currentLocation)
                    .AbsoluteUri;
            }
        }

        string liveConfigJson = JsonConvert.SerializeObject(liveExistingConfigModel, Formatting.Indented);
        File.WriteAllText(configFileName, liveConfigJson);
    }

    public static void WriteFileNameAndCoordinatesToJsonFile(string coordinates, string fileName)
    {
        string jsonCoordinates = WriteFileNameAndCoordinatesToJson(coordinates, fileName);

        if (!string.IsNullOrEmpty(jsonCoordinates))
            File.WriteAllText(fileName, jsonCoordinates);
    }

    public static string WriteFileNameAndCoordinatesToJson(string coordinates, string fileName)
    {
        string[] aryCoordinates = coordinates.Split(',');

        if (aryCoordinates.Length > 1)
        {
            var objCoordinates = new
            {
                lat = Convert.ToDouble(aryCoordinates[1].Trim()
                    , new NumberFormatInfo { NumberDecimalSeparator = "." })
                ,
                lng = Convert.ToDouble(aryCoordinates[0].Trim()
                    , new NumberFormatInfo { NumberDecimalSeparator = "." })
            };
            return JsonSerializer.Serialize(objCoordinates);
        }

        return string.Empty;
    }

    public static void WriteConfigurationToJsonFile(string strAbsolutePath, string kmlFileNameWithRelativePath, string configFileName)
    {
        string jsonConfiguration = WriteConfigurationToJson(strAbsolutePath, kmlFileNameWithRelativePath);

        if (!string.IsNullOrEmpty(jsonConfiguration))
            File.WriteAllText(configFileName, jsonConfiguration);
    }

    public static string WriteConfigurationToJson(string strAbsolutePath, string kmlFileNameWithRelativePath)
    {
        Uri kmlUrl =
            ConvertRelativeWindowsPathToUri(strAbsolutePath, kmlFileNameWithRelativePath);
        Uri currentLocationJson = ConvertRelativeWindowsPathToUri(strAbsolutePath, "test.json");
        var objConfig = new
        {
            kmlUrl = kmlUrl.AbsoluteUri
            , currentLocation = currentLocationJson.AbsoluteUri
        };
        return JsonSerializer.Serialize(objConfig);
    }

    public static void WriteConfigurationToJsonFileForBlog(string strAbsolutePath, string kmlFileNameWithRelativePath, string configFileName)
    {
        string jsonConfiguration = WriteConfigurationToJsonForBlog(strAbsolutePath, kmlFileNameWithRelativePath);

        if (!string.IsNullOrEmpty(jsonConfiguration))
            File.WriteAllText(configFileName, jsonConfiguration);
    }

    public static string WriteConfigurationToJsonForBlog(string strAbsolutePath, string kmlFileNameWithRelativePath)
    {
        Uri kmlUrl =
            ConvertRelativeWindowsPathToUri(strAbsolutePath, kmlFileNameWithRelativePath);
        var objConfig = new
        {
            kmlUrl = kmlUrl.AbsoluteUri
        };
        return JsonSerializer.Serialize(objConfig);
    }

    public static Uri ConvertRelativeWindowsPathToUri(string strAbsolutePath, string relativeWindowsPath)
    {
        Uri uriAbsolute = new Uri(strAbsolutePath);
        return new Uri(uriAbsolute, relativeWindowsPath);
    }

    public static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }

    public static string GetValue(JObject data, string value)
    {
        return (data[value] ?? string.Empty).Value<string>() ?? string.Empty;
    }
}