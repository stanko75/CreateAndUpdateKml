using Common;
using KmlSerializer;

namespace CreateAndUpdateKmlLib.Tests;

[TestClass]
public class UpdateKmlIfExistsOrCreateNewIfNotTests
{
    private readonly string _fileName = "testhost.kml";
    private IUpdateKmlIfExistsOrCreateNewIfNot _updateKmlIfExistsOrCreateNewIfNot;

    [TestInitialize]
    public void Initialize()
    {
        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }

        _updateKmlIfExistsOrCreateNewIfNot = new UpdateKmlIfExistsOrCreateNewIfNot(new UpdateKml()
            , new CreateKml("test", "test")
            , new KmlSerializerTextWriter(typeof(KmlModel.Kml)));

        _updateKmlIfExistsOrCreateNewIfNot.Execute(_fileName, "12.8965296, 48.8417986, 2357");
    }

    [TestMethod]
    public void WhenFileNotExistCheckIfItWillBeCreated()
    {
        Assert.IsTrue(File.Exists(_fileName));
    }

    [TestMethod]
    public void WhenFileExistCheckIfItWillBeUpdated()
    {
        CreateKml kml = new CreateKml("test", "unit test");

        KmlSerializerTextWriter kmlSerializerTextWriter = new KmlSerializerTextWriter(typeof(KmlModel.Kml));
        SaveKml saveKml = new SaveKml(kml, kmlSerializerTextWriter);
        saveKml.Execute(_fileName);

        _updateKmlIfExistsOrCreateNewIfNot.Execute(_fileName, "12.8965296, 48.8417986, 2357");

        KmlModel.Kml? kmlToUpdate = kmlSerializerTextWriter.DoDeserialization(_fileName);
        Assert.IsTrue(kmlToUpdate?.Document?.Placemarks?[0].LineString?.Coordinates?.Equals("12.8965296, 48.8417986, 2357"));
    }

    [TestMethod]
    public void CheckIfExceptionWillBeRaisedWhenCoordinatesAreEmpty()
    {
        CreateKml kml = new CreateKml("test", "unit test");

        KmlSerializerTextWriter kmlSerializerTextWriter = new KmlSerializerTextWriter(typeof(KmlModel.Kml));
        SaveKml saveKml = new SaveKml(kml, kmlSerializerTextWriter);
        saveKml.Execute(_fileName);

        try
        {
            _updateKmlIfExistsOrCreateNewIfNot.Execute(_fileName, "");
        }
        catch (Exception ex)
        {
            Assert.IsTrue(ex.Message.Equals("Coordinates cannot be empty!"));
        }

    }
}