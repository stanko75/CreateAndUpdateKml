namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
[TestClass]
public class ResizeImageTests
{
    [TestMethod]
    public void ResizeImageAndCheckIfExist()
    {
        ResizeImage resizeImage = new ResizeImage();
        resizeImage.Execute(
            "excalibur.jpg"
            , "excaliburResized.jpg"
            , 200
            , 200
        );

        Assert.IsTrue(File.Exists("excaliburResized.jpg"));
    }
}