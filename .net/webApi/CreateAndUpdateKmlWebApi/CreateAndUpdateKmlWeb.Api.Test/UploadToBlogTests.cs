using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CreateAndUpdateKmlWeb.Api.Test;

[TestClass]
public class UploadToBlogTests
{
    private readonly string _address;

    public UploadToBlogTests()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("jsconfigForTests.json")
            .Build();

        string? address = configuration.GetSection("address").Value;
        if (address is not null) _address = address;

        //string? gpsLocationsPath = configuration.GetSection("gpsLocationsPath").Value;
    }

    [TestMethod]
    public void GetKmlAndCheckIfNotNull()
    {
        JObject androidConfiguration = new JObject
        {
            ["folderName"] = "album1",
            ["fileName"] = "test"
        };

        HttpClient httpClient = new HttpClient();
        Task<HttpResponseMessage> httpResponseMessage = Task.Run(() => httpClient.PostAsync(
            Path.Combine(_address, "api/UpdateCoordinates/UploadToBlog")
            , new StringContent($"{androidConfiguration}"
                , Encoding.UTF8
                , "text/json")));
        httpResponseMessage.Wait();

        Task<string> httpClientPostResult = httpResponseMessage.Result.Content.ReadAsStringAsync();

        HttpResponseMessage httpResponseMessage1 = httpResponseMessage.Result;
        string result = httpResponseMessage1.Content.ReadAsStringAsync().Result;
        Thread.Sleep(2000);
    }

    [TestMethod]
    public void UploadCustomAlbum()
    {
        JObject androidConfiguration = new JObject
        {
            ["folderName"] = "walk",
            ["fileName"] = @"walk\ahr.kml"
        };

        HttpClient httpClient = new HttpClient();
        Task<HttpResponseMessage> httpResponseMessage = Task.Run(() => httpClient.PostAsync(
            Path.Combine(_address, "api/UpdateCoordinates/UploadToBlog")
            , new StringContent($"{androidConfiguration}"
                , Encoding.UTF8
                , "text/json")));
        httpResponseMessage.Wait();;

        Task<string> httpClientPostResult = httpResponseMessage.Result.Content.ReadAsStringAsync();

        HttpResponseMessage httpResponseMessage1 = httpResponseMessage.Result;
        httpResponseMessage1.Content.ReadAsStringAsync().Wait();

        while (httpResponseMessage.Status != TaskStatus.RanToCompletion)
        {
            Thread.Sleep(2000);
        }
        string url = $"http://www.milosev.com/gallery/allWithPics/travelBuddies/{androidConfiguration["folderName"]}/index.html";
        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c start {url}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true
        });
    }
}