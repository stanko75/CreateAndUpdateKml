using Microsoft.Extensions.Configuration;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class PrepareHmlFilesAndUploadTests
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
        string kmlFileName = "test.kml";

        if (_configuration is null)
        {
            Assert.Fail("Configuration is null");
            return;
        }

        string host = _configuration.GetSection("host").Value ?? string.Empty;
        string user = _configuration.GetSection("user").Value ?? string.Empty;
        string pass = _configuration.GetSection("pass").Value ?? string.Empty;

        PrepareHmlFilesAndUpload prepareHmlFilesAndUpload = new PrepareHmlFilesAndUpload(
            new PrepareHtmlFiles()
            , new MirrorDirAndFileStructureOnFtp(
                new FtpUpload(host, user, pass)
            )
            , new WriteConfigurationToJsonFile()
        );

        prepareHmlFilesAndUpload.Execute(
            @"..\..\..\..\..\..\html\blog"
            , "prepareForUpload"
            , "nameOfAlbum"
            , kmlFileName
            , "public_html/mirrorDirAndFileStructureOnFtpDelete"
        );
    }
}