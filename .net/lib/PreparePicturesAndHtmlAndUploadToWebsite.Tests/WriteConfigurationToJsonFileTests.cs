using Newtonsoft.Json.Linq;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class WriteConfigurationToJsonFileTests
{
    [TestInitialize]
    public void Initialize()
    {
        if (File.Exists("config.json"))
        {
            File.Delete("config.json");
        }
    }

    [TestMethod]
    public void CreateConfigurationFileAndCheckIfExists()
    {
        WriteConfigurationToJsonFile writeConfigurationToJsonFile = new WriteConfigurationToJsonFile();
        writeConfigurationToJsonFile.Execute("https://www.milosev.com", "test.kml", "config.json");
        Assert.IsTrue(File.Exists("config.json"));
    }

    [TestMethod]
    public void CreateConfigurationFileAndCheckIfJsonIsValid()
    {
        CreateConfigurationFileAndCheckIfExists();
        JObject configJson = JObject.Parse(File.ReadAllText("config.json"));
        Assert.IsNotNull(configJson);
        Assert.IsTrue(configJson["KmlFileName"].ToString().Equals("https://www.milosev.com/test.kml"), $"Value is {configJson["kmlUrl"]}");
    }

    [TestMethod]
    public void CreateConfigurationFileLikeOnAzureAndCheckIfJsonIsValid()
    {
        Directory.CreateDirectory(@"prepareForUpload\album1");

        WriteConfigurationToJsonFile writeConfigurationToJsonFile = new WriteConfigurationToJsonFile();
        writeConfigurationToJsonFile.Execute($"https://www.milosev.com/gallery/allWithPics/travelBuddies/album1/"
            , "test.kml"
            , @"prepareForUpload\album1\config.json");
        Assert.IsTrue(File.Exists(@"prepareForUpload\album1\config.json"));

        JObject configJson = JObject.Parse(File.ReadAllText(@"prepareForUpload\album1\config.json"));
        Assert.IsNotNull(configJson);
        Assert.IsTrue(configJson["KmlFileName"].ToString().Equals("https://www.milosev.com/gallery/allWithPics/travelBuddies/album1/test.kml"), $"Value is {configJson["kmlUrl"]}");
    }

    [TestMethod]
    public void CreateConfigurationFileWithPathToKmlAndCheckIfExists()
    {
        WriteConfigurationToJsonFile writeConfigurationToJsonFile = new WriteConfigurationToJsonFile();
        writeConfigurationToJsonFile.Execute("https://www.milosev.com", "test\\test.kml", "config.json");
        Assert.IsTrue(File.Exists("config.json"));
    }

    [TestMethod]
    public void CreateConfigurationFileWithPathToKmlAndCheckIfJsonIsValid()
    {
        CreateConfigurationFileWithPathToKmlAndCheckIfExists();
        JObject configJson = JObject.Parse(File.ReadAllText("config.json"));
        Assert.IsNotNull(configJson);
        Assert.IsTrue(configJson["KmlFileName"].ToString().Equals("https://www.milosev.com/test.kml"), $"Value is {configJson["kmlUrl"]}");
    }
}