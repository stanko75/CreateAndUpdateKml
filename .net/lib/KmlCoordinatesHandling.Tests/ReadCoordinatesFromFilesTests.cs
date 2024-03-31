namespace KmlCoordinatesHandling.Tests;

[TestClass]
public class ReadCoordinatesFromFilesTests
{
    private readonly ReadCoordinatesFromFiles _readCoordinatesFromFiles;

    public ReadCoordinatesFromFilesTests()
    {
        _readCoordinatesFromFiles = new ReadCoordinatesFromFiles();
    }


    [TestMethod]
    public void PickFromFiles()
    {
        string coordinate = _readCoordinatesFromFiles.GenerateCoordinates("locations");
        Assert.AreEqual(coordinate
            , "7.2553877, 50.4040326, 2357 " + Environment.NewLine
            + "7.2553499, 50.4040365, 2357 " + Environment.NewLine
            + "7.2550569, 50.4039482, 2357 " + Environment.NewLine
            + "7.2549359, 50.4039275, 2357 " + Environment.NewLine
            + "7.2546084, 50.403897, 2357 " + Environment.NewLine
        );
    }
}