using System.Net;
using Microsoft.Extensions.Configuration;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class FtpUploadTests
{
    private IConfigurationRoot? _configuration;

    [TestInitialize]
    public void Initialize()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("jsconfigForTests.json")
            .Build();
    }

    [TestMethod]
    public void UploadFileAndCheckIfExists()
    {
        UploadFileAndCheckIfExistsAsync().GetAwaiter().GetResult();
    }

    private async Task UploadFileAndCheckIfExistsAsync()
    {
        string remoteFolder = "kmlTestDelete";
        string remoteFileName = $"/public_html/{remoteFolder}/test.kml";
        string fileName = "test.kml";

        if (!File.Exists(fileName))
        {
            Assert.Fail("File not exist");
            return;
        }

        if (_configuration is null)
        {
            Assert.Fail("Configuration is null");
            return;
        }

        CreateNewFolderAsync(remoteFolder).GetAwaiter().GetResult();

        string host = _configuration.GetSection("host").Value ?? string.Empty;
        string user = _configuration.GetSection("user").Value ?? string.Empty;
        string pass = _configuration.GetSection("pass").Value ?? string.Empty;

        FtpUpload ftpDelete = new(host, user, pass);
        Task<string> ftpDeleteTask = ftpDelete.FtpFileDeleteAsync(remoteFileName);
        string deleteExceptionMessage = await ftpDeleteTask;
        //Assert.IsTrue(string.IsNullOrWhiteSpace(deleteExceptionMessage), $"Delete exception message: {deleteExceptionMessage}");

        FtpUpload ftpUpload = new(host, user, pass);
        Task<string> ftpTask = ftpUpload.FtpFileUploadAsync(fileName, remoteFileName);
        string exceptionMessage = await ftpTask;
        Assert.IsTrue(string.IsNullOrWhiteSpace(exceptionMessage), $"Upload exception message: {exceptionMessage}");

        Uri domain = new UriBuilder(host.Replace("ftp", "www")).Uri;

        if (Uri.TryCreate(domain, remoteFileName.Replace(@"/public_html", string.Empty), out Uri? myUri))
        {
            HttpClient httpClientGet = new HttpClient();
            Task<HttpResponseMessage> httpClientGetResult = httpClientGet.GetAsync(myUri.AbsoluteUri);
            HttpResponseMessage result = httpClientGetResult.GetAwaiter().GetResult();
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
        }

    }

    [TestMethod]
    public void CreateNewFolder()
    {
        CreateNewFolderAsync("notExistingFolderDelete").GetAwaiter().GetResult();
    }

    private async Task CreateNewFolderAsync(string folderName)
    {
        string fileName = "test.kml";

        if (!File.Exists(fileName))
        {
            Assert.Fail("File not exist");
            return;
        }

        if (_configuration is null)
        {
            Assert.Fail("Configuration is null");
            return;
        }

        string host = _configuration.GetSection("host").Value ?? string.Empty;
        string user = _configuration.GetSection("user").Value ?? string.Empty;
        string pass = _configuration.GetSection("pass").Value ?? string.Empty;

        FtpUpload ftpUpload = new(host, user, pass);
        Task<string> ftpTask = ftpUpload.FtpFileCreateDirectoryAsync($"/public_html/{folderName}");
        string exceptionMessage = await ftpTask;
        Assert.IsTrue(string.IsNullOrWhiteSpace(exceptionMessage), $"Exception message: {exceptionMessage}");
    }
}