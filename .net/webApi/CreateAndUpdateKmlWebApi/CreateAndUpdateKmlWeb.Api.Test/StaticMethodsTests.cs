using System.Net;
using CreateAndUpdateKmlWebApi;
using CreateAndUpdateKmlWebApi.Models;
using Newtonsoft.Json.Linq;

namespace CreateAndUpdateKmlWeb.Api.Test;

[TestClass]
public class StaticMethodsTests
{
    string strAbsolutePath = @"https://milosevtracking.azurewebsites.net";
    string kmlFileName = @"default\default.kml";

    [TestMethod]
    public void ConvertRelativeWindowsPathToUriTest()
    {
        Uri expectedResult = new Uri($@"{strAbsolutePath}/default/default.kml");
        Assert.AreEqual(expectedResult
            , CommonStaticMethods.ConvertRelativeWindowsPathToUri(strAbsolutePath, kmlFileName));
    }

    //WriteConfigurationToJson
    [TestMethod]
    public void WriteConfigurationToJsonTest()
    {
        string expectedResult = $"{{\"kmlUrl\":\"{strAbsolutePath}/default/default.kml\",\"currentLocation\":\"{strAbsolutePath}/test.json\"}}";

        Assert.AreEqual(expectedResult
            , CommonStaticMethods.WriteConfigurationToJson(strAbsolutePath, kmlFileName));
    }

    [TestMethod]
    public void WriteConfigurationToJsonFileTest()
    {
        string configFileName = "config.json";
        CommonStaticMethods.WriteConfigurationToJsonFile(strAbsolutePath, kmlFileName, "config.json");
        Assert.IsTrue(File.Exists(configFileName));

    }

    [TestMethod]
    public void WriteConfigurationModelToJsonLoadFileInTestAndCompareResults()
    {
        string testConfigFileName = "config.json";
        string configFileNameUnderTest = "configUnderTest.json";

        string strExistingConfig = File.ReadAllText(testConfigFileName);
        JObject jsonObjectExistingConfig = JObject.Parse(strExistingConfig);
        LiveConfigModel existingConfigModel = jsonObjectExistingConfig.ToObject<LiveConfigModel>() ??
                                              throw new InvalidOperationException();

        LiveConfigModel configModelUnderTest = new()
        {
            CurrentLocation = "test.json"
            , KmlFileName = "default.kml"
            , LiveImageMarkersJsonUrl = "defaultThumbs.json"
        };

        string folderName = "default";
        string rootUrl =
            "http://livetracking.milosev.com:100/.net/webApi/CreateAndUpdateKmlWebApi/CreateAndUpdateKmlWebApi";

        CommonStaticMethods.WriteConfigurationToJsonFile(folderName
            , "default.kml"
            , "test.json"
            , configFileNameUnderTest
            , rootUrl);

        string strConfigFileNameUnderTest = File.ReadAllText(configFileNameUnderTest);
        JObject jsonObjectConfigFileNameUnderTest = JObject.Parse(strConfigFileNameUnderTest);
        configModelUnderTest = jsonObjectConfigFileNameUnderTest.ToObject<LiveConfigModel>() ??
                               throw new InvalidOperationException();

        Assert.AreEqual(existingConfigModel.CurrentLocation, configModelUnderTest.CurrentLocation);
        Assert.AreEqual(existingConfigModel.KmlFileName, configModelUnderTest.KmlFileName);
        Assert.AreEqual(existingConfigModel.LiveImageMarkersJsonUrl, configModelUnderTest.LiveImageMarkersJsonUrl);
    }
}