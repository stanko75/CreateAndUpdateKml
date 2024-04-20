using CreateAndUpdateKmlWebApi.Models;
using Newtonsoft.Json.Linq;

namespace CreateAndUpdateKmlWeb.Api.Test;

[TestClass]
public class ImageModelTests
{
    [TestMethod]
    public void CheckIfModelIsProperyPrepared()
    {
        JObject data = new JObject();
        byte[] bytes = { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 };
        data["base64Image"] = Convert.ToBase64String(bytes);
        data["folderName"] = "testFolderName";
        data["imageFileName"] = "testImage.jpg";
        data["kmlFileName"] = "testKml.kml";

        KmlFileFolderModel kmlFileFolderModel = new KmlFileFolderModel(data);
        ImageModel imageModel = new ImageModel(kmlFileFolderModel, data);
        Assert.AreEqual(imageModel.ImageThumbsFileName, "testFolderName\\thumbs\\testImage.jpg");
        Assert.AreEqual(imageModel.ImageThumbsFolderName, "testFolderName\\thumbs");
        Assert.AreEqual(imageModel.JsonFileName, "testFolderName\\testKmlThumbs.json");
        Assert.AreEqual(imageModel.NameOfFileForJson, "../thumbs/testImage.jpg");
    }
}