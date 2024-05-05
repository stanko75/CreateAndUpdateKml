using Newtonsoft.Json.Linq;

namespace CreateAndUpdateKmlWebApi.Models;

public class ImageModel
{
    public ImageModel(KmlFileFolderModel kmlFileFolderModel, JObject data, string rootUrl)
    {
        string thumbsFolder = "thumbs";

        Base64Image = data["base64Image"]?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(Base64Image))
        {
            throw new Exception("Error: No image." );
        }

        ImageOriginalFolderName = Path.Join(kmlFileFolderModel.FolderName, "pics");
        Directory.CreateDirectory(ImageOriginalFolderName);

        ImageThumbsFolderName = Path.Join(kmlFileFolderModel.FolderName, thumbsFolder);
        Directory.CreateDirectory(ImageThumbsFolderName);

        string? imageFileName = data["imageFileName"]?.ToString();
        imageFileName ??= "default.jpg";
        ImageThumbsFileName = $"{ImageThumbsFolderName}\\{imageFileName}";
        //NameOfFileForJson = $"../{thumbsFolder}/{imageFileName}"; //ToDO
        NameOfFileForJson = $"{rootUrl}/{ImageThumbsFolderName.Replace('\\', '/')}/{imageFileName}";

        string jsonFileName = Path.GetFileNameWithoutExtension(kmlFileFolderModel.KmlFileName);
        jsonFileName = string.IsNullOrWhiteSpace(jsonFileName) ? "default" : jsonFileName;
        FileNameThumbsJson = Path.ChangeExtension($"{jsonFileName}Thumbs", "json");
        FileNameThumbsJson = Path.Join(kmlFileFolderModel.FolderName, FileNameThumbsJson);

        ImageOriginalFileName = Path.Join(ImageOriginalFolderName, imageFileName);
        ImageBytes = Convert.FromBase64String(Base64Image);
    }

    public string Base64Image { get; set; }
    public string ImageThumbsFolderName { get; set; }
    public string ImageThumbsFileName { get; set; }
    public string NameOfFileForJson { get; set; }
    public string FileNameThumbsJson { get; set; }
    public string ImageOriginalFolderName { get; set; }
    public string ImageOriginalFileName { get; set; }
    public byte[] ImageBytes { get; set; }
}