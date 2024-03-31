using System.Net;
using CreateAndUpdateKmlWebApi;

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
}