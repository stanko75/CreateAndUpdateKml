namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class ExtractGpsInfoFromImageTests
{
    [TestMethod]
    public void ExtractGpsInfoAndCheckIfCorrect()
    {
        ExtractGpsInfoFromImage extractGpsInfoFromImage = new ExtractGpsInfoFromImage();
        LatLngFileNameModel? latLngFileNameModel =
            extractGpsInfoFromImage.Execute("IMG_20230325_114038.jpg", "IMG_20230325_114038.jpg");
        Assert.IsNotNull(latLngFileNameModel);
        Assert.AreEqual(latLngFileNameModel.lat, 50.7406425475);
        Assert.AreEqual(latLngFileNameModel.lng, 7.0952768325);
    }
}