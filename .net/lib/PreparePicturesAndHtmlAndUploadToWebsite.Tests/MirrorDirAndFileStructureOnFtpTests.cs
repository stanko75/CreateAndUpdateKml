using Microsoft.Extensions.Configuration;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class MirrorDirAndFileStructureOnFtpTests
{
    private IConfigurationRoot? _configuration;
    private readonly List<string> _listOfFilesAndFoldersToPrepareForUpload = new();

    private string _host = string.Empty;
    private string _user = string.Empty;
    private string _pass = string.Empty;
    private readonly string _remoteRootFolder = "public_html/mirrorDirAndFileStructureOnFtpDelete";
    private readonly string _localRootFolder = "mirrorDirAndFileStructureOnFtpTests";

    [TestInitialize]
    public void Initialize()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("jsconfigForTests.json")
            .Build();

        _host = _configuration.GetSection("host").Value ?? string.Empty;
        _user = _configuration.GetSection("user").Value ?? string.Empty;
        _pass = _configuration.GetSection("pass").Value ?? string.Empty;

        CreateComplicatedStructureToMirror();
    }

    private void CreateComplicatedStructureToMirror()
    {
        _listOfFilesAndFoldersToPrepareForUpload.Add(@$"{_localRootFolder}/file1.txt");
        _listOfFilesAndFoldersToPrepareForUpload.Add($@"{_localRootFolder}/folder1/file1.txt");
        _listOfFilesAndFoldersToPrepareForUpload.Add($@"{_localRootFolder}/folder2/file1.txt");
        _listOfFilesAndFoldersToPrepareForUpload.Add(
            $@"{_localRootFolder}/folder1/subfolder1/file1.txt");
        _listOfFilesAndFoldersToPrepareForUpload.Add(
            @$"{_localRootFolder}/folder1/subfolder2/file1.txt");
        _listOfFilesAndFoldersToPrepareForUpload.Add(
            @$"{_localRootFolder}/folder2/subfolder1/file1.txt");
        _listOfFilesAndFoldersToPrepareForUpload.Add(
            @$"{_localRootFolder}/folder2/subfolder1/subSubfolder1/file1.txt");
        _listOfFilesAndFoldersToPrepareForUpload.Add(
            @$"{_localRootFolder}/folder2/subfolder2/subSubfolder1/file1.txt");

        async Task<string> FtpDeleteFileTask(string remoteFileName)
        {
            FtpUpload ftpDelete = new(_host, _user, _pass);
            Task<string> ftpDeleteTask = ftpDelete.FtpFileDeleteAsync(remoteFileName);
            string deleteExceptionMessage = await ftpDeleteTask;
            return deleteExceptionMessage;
        }

        async Task<string> FtpDeleteDirectoryTask(string remoteFileName)
        {
            FtpUpload ftpDelete = new(_host, _user, _pass);
            Task<string> ftpDeleteTask = ftpDelete.FtpDirectoryDeleteAsync(remoteFileName);
            string deleteExceptionMessage = await ftpDeleteTask;
            return deleteExceptionMessage;
        }

        _listOfFilesAndFoldersToPrepareForUpload.Reverse();
        foreach (string fileAndFolder in _listOfFilesAndFoldersToPrepareForUpload)
        {
            string pathWithoutRoot = fileAndFolder.Replace(_localRootFolder + "/", string.Empty);
            Task<string> ftpDeleteFileTask = FtpDeleteFileTask($"{_remoteRootFolder}/{pathWithoutRoot}");
            ftpDeleteFileTask.Wait();
            string? folderName = Path.GetDirectoryName(pathWithoutRoot);
            if (string.IsNullOrWhiteSpace(folderName)) continue;
            Task<string> ftpDeleteDirectoryTask = FtpDeleteDirectoryTask($"{_remoteRootFolder}/{folderName}");
            ftpDeleteDirectoryTask.Wait();
            Directory.CreateDirectory(folderName);
            File.WriteAllText(fileAndFolder, fileAndFolder);
        }
    }

    [TestMethod]
    public void CreateMirrorStructTest()
    {
        CreateMirrorStruct().GetAwaiter().GetResult();
    }

    private async Task CreateMirrorStruct()
    {
        if (_configuration is null)
        {
            Assert.Fail("Configuration is null");
            return;
        }

        MirrorDirAndFileStructureOnFtp mirrorDirAndFileStructureOnFtp =
            new MirrorDirAndFileStructureOnFtp(new FtpUpload(_host, _user, _pass));
        Task? mirrorDirAndFileStructureOnFtpTask = mirrorDirAndFileStructureOnFtp.Execute(_localRootFolder,
        _remoteRootFolder);
        if (mirrorDirAndFileStructureOnFtpTask is not null)
        {
            await mirrorDirAndFileStructureOnFtpTask;
        }
        foreach (string fileAndFolder in _listOfFilesAndFoldersToPrepareForUpload)
        {
            string pathWithoutRoot = fileAndFolder.Replace(_localRootFolder + "/", string.Empty);

            Uri domain = new UriBuilder(_host.Replace("ftp", "www")).Uri;
            if (!Uri.TryCreate(domain, _remoteRootFolder.Replace(@"public_html", string.Empty),
                    out Uri? myUri)) continue;
            HttpClient httpClientGet = new HttpClient();
            Task<HttpResponseMessage> httpClientGetResult = httpClientGet.GetAsync($"{myUri.AbsoluteUri}/{pathWithoutRoot}");
            HttpResponseMessage result = httpClientGetResult.GetAwaiter().GetResult();
            Assert.IsTrue(result.StatusCode == HttpStatusCode.OK, $"Actual result: {result.StatusCode}, uri: {myUri.AbsoluteUri}/{pathWithoutRoot}");
        }
    }
}