using Newtonsoft.Json.Linq;

namespace CreateAndUpdateKmlWebApi.Models;

public class KmlFileFolderModel
{
    public KmlFileFolderModel(JObject data)
    {
        string rootFolderName = CommonStaticMethods.GetValue(data, "folderName");
        FolderName = string.IsNullOrWhiteSpace(rootFolderName) ? "default" : rootFolderName;

        string kmlFileName = CommonStaticMethods.GetValue(data, "kmlFileName");
        KmlFileName = string.IsNullOrWhiteSpace(kmlFileName) ? "default" : kmlFileName;
        KmlFileName = Path.ChangeExtension(KmlFileName, "kml");
    }

    public string FolderName { get; set; }
    public string KmlFileName { get; set; }
}