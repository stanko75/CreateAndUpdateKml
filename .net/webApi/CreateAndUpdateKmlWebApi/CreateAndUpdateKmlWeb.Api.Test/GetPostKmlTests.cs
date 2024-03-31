using System.Text;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace CreateAndUpdateKmlWeb.Api.Test;

[TestClass]
public class GetPostKmlTests
{
    private readonly string _address;
    private readonly string _gpsLocationsPath;

    public GetPostKmlTests()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("jsconfigForTests.json")
            .Build();

        string? address = configuration.GetSection("address").Value;
        if (address is not null) _address = address;

        string? gpsLocationsPath = configuration.GetSection("gpsLocationsPath").Value;
        if (gpsLocationsPath is not null) _gpsLocationsPath = gpsLocationsPath;
    }

    [TestMethod]
    public void GetKmlAndCheckIfNotNull()
    {
        HttpClient httpClient = new HttpClient();
        Task<string> kml = httpClient.GetStringAsync(Path.Combine(_address, "test.kml"));
        Assert.IsNotNull(kml.GetAwaiter().GetResult());
    }

    [TestMethod]
    public void PostGpsPositions()
    {
        HttpClient httpClient = new HttpClient();
        Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(
            Path.Combine(_address, @"api/UpdateCoordinates")
            , new StringContent(@"""string Test"""
                , Encoding.UTF8
                , "text/json"));

        Task<string> httpClientPostResult = httpResponseMessage.Result.Content.ReadAsStringAsync();
        string result = httpClientPostResult.Result;
        Assert.IsNotNull(result);

        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load("https://localhost:7293/test.kml");
        Assert.IsNotNull(xmlDocument);

        HttpClient httpClientGet = new HttpClient();
        Task<string> httpClientGetResult = httpClientGet.GetStringAsync(@"https://localhost:7293/test.txt");
        Assert.IsFalse(string.IsNullOrWhiteSpace(httpClientGetResult.GetAwaiter().GetResult()));
    }

    [TestMethod]
    public void PostGpsPositionsFromFiles()
    {
        string? result = null;

        using HttpClient httpClient = new HttpClient();
        if (Directory.Exists(_gpsLocationsPath))
        {
            foreach (string file in Directory.GetFiles(_gpsLocationsPath))
            {
                JObject myJObject = JObject.Parse(File.ReadAllText(file));
                string gpsLocation = $"{myJObject["lng"]}, {myJObject["lat"]}, 2357 ";

                Task<HttpResponseMessage> task = Task.Run(() => httpClient.PostAsync(
                    Path.Combine(_address, @"api/UpdateCoordinates")
                    , new StringContent(@$"""{gpsLocation}"""
                        , Encoding.UTF8
                        , "text/json")));
                task.Wait();

                HttpResponseMessage httpResponseMessage = task.Result;
                result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                Thread.Sleep(2000);
            }
        }

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void PostGpsPositionsWithFileName()
    {
        JObject o = new JObject();
        o["folderName"] = "album1";
        o["fileName"] = "test";
        o["coordinates"] = "7.0881042, 50.7541783, 2357";

        HttpClient httpClient = new HttpClient();

        Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(
            Path.Combine(_address, @"api/UpdateCoordinates/PostFileFolder")
            , new StringContent($@"{o}"
                , Encoding.UTF8
                , "text/json"));

        Task<string> httpClientPostResult = httpResponseMessage.Result.Content.ReadAsStringAsync();
        string result = httpClientPostResult.Result;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void PostGpsPositionsFromFilesWithFileName()
    {
        JObject o = new JObject();
        o["folderName"] = "album1";
        o["fileName"] = "test";

        string? result = null;

        using HttpClient httpClient = new HttpClient();
        if (Directory.Exists(_gpsLocationsPath))
        {
            foreach (string file in Directory.GetFiles(_gpsLocationsPath))
            {
                JObject myJObject = JObject.Parse(File.ReadAllText(file));
                //o["coordinates"] = $"{myJObject["lng"]}, {myJObject["lat"]}, 2357 ";
                o["lng"] = myJObject["lng"];
                o["lat"] = myJObject["lat"];

                Task<HttpResponseMessage> task = Task.Run(() => httpClient.PostAsync(
                    Path.Combine(_address, @"api/UpdateCoordinates/PostFileFolder")
                    , new StringContent($@"{o}"
                        , Encoding.UTF8
                        , "text/json")));
                task.Wait();

                HttpResponseMessage httpResponseMessage = task.Result;
                result = httpResponseMessage.Content.ReadAsStringAsync().Result;
                Thread.Sleep(2000);
            }
        }

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void PostNonsense()
    {
        HttpClient httpClient = new HttpClient();
        JObject o = new JObject();

        Task<HttpResponseMessage> httpResponseMessage = httpClient.PostAsync(
            Path.Combine(_address, @"api/UpdateCoordinates/PostFileFolder")
            , new StringContent($@"{o}"
                , Encoding.UTF8
                , "text/json"));

        Task<string> httpClientPostResult = httpResponseMessage.Result.Content.ReadAsStringAsync();
        string result = httpClientPostResult.Result;

        Assert.IsNotNull(result);
    }
}