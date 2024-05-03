namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class CopyHtmlFilesTests
{
    [TestMethod]
    public void RaiseDirectoryNotFoundException()
    {
        CopyHtmlFiles copyHtmlFiles = new CopyHtmlFiles();
        DirectoryNotFoundException directoryNotFoundException = Assert.ThrowsException<DirectoryNotFoundException>(() =>
            copyHtmlFiles.CopyHtmlTemplateForBlog(@"html\blog1", "prepareForUpload", "nameOfAlbum", "test.kml"));
        Assert.AreEqual(directoryNotFoundException.Message,
            @"The folder C:\projects\CreateAndUpdateKml\.net\lib\PreparePicturesAndHtmlAndUploadToWebsite.Tests\bin\Debug\net8.0\html\blog1 does not exist!");
    }

    [TestMethod]
    public void RaiseFileNotFoundException()
    {
        string fileName = "test.kml";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        CopyHtmlFiles copyHtmlFiles = new CopyHtmlFiles();
        FileNotFoundException fileNotFoundException = Assert.ThrowsException<FileNotFoundException>(() =>
            copyHtmlFiles.CopyHtmlTemplateForBlog(@"..\..\..\..\..\..\html\blog", "prepareForUpload", "nameOfAlbum",
                fileName));
        Assert.AreEqual(fileNotFoundException.Message,
            "The file test.kml does not exist. Absolute path: C:\\projects\\CreateAndUpdateKml\\.net\\lib\\PreparePicturesAndHtmlAndUploadToWebsite.Tests\\bin\\Debug\\net8.0\\test.kml");
    }

    [TestMethod]
    public void SimulateRealLifeSituation()
    {
        string folder = "album1";
        string fileName = @"album1\test";

        string extension = ".kml";
        fileName = CommonMethodsForTests.Common.ChangeFileExtension(fileName, extension);

        CopyHtmlFiles copyHtmlFiles = new CopyHtmlFiles();
        copyHtmlFiles.CopyHtmlTemplateForBlog(@"html\blog"
            , "prepareForUpload"
            , folder
            , fileName);
        string pathToKmlShouldBe = Path.Join("prepareForUpload", Path.GetDirectoryName(fileName));
        Assert.IsTrue(Directory.Exists(pathToKmlShouldBe), $"File: {Path.GetFullPath(pathToKmlShouldBe)} not exists.");
    }
}