using Common;
using KmlSerializer;

namespace CreateAndUpdateKmlLib.Tests;

[TestClass]
public class UpdateKmlTests
{
    private readonly UpdateKml _updateKml;

    public UpdateKmlTests()
    {
        _updateKml = new UpdateKml();
        CreateKml kml = new CreateKml("test", "unit test");
        CommonMethodsForTests.Common.CreateKml(kml);
        _updateKml.OldKml = kml.GenerateKml();
    }

    [TestMethod]
    public void CheckIfItsNotNull()
    {
        KmlModel.Kml? result = _updateKml.GenerateKml();
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void CheckIfStyleIdIsChanged()
    {
        Assert.IsTrue(_updateKml.OldKml?.Document?.Style?.Id?.Equals("red"));

        _updateKml.Style = new KmlModel.Style
        {
            Id = "test"
        };
        KmlModel.Kml? result = _updateKml.GenerateKml();
        Assert.IsTrue(result?.Document?.Style?.Id?.Equals("test"));
    }

    [TestMethod]
    public void UpdateCoordinates()
    {
        Assert.IsTrue(_updateKml.OldKml?.Document?.Placemarks?[0].LineString?.Coordinates?.Equals("12.8977216, 48.8408876, 2357, 12.8965296, 48.8417986, 2357, 12.8953309, 48.8426691, 2357"));
        _updateKml.Placemark = new KmlModel.Placemark
        {
            LineString = new KmlModel.LineString
            {
                Coordinates = "12.8977216, 48.8408876, 2357"
            }
        };
        KmlModel.Kml? result = _updateKml.GenerateKml();
        Assert.IsTrue(result?.Document?.Placemarks?[0].LineString?.Coordinates?.Equals("12.8977216, 48.8408876, 2357, 12.8965296, 48.8417986, 2357, 12.8953309, 48.8426691, 2357, 12.8977216, 48.8408876, 2357"));
    }

    [TestMethod]
    public void CreateAndSaveEmptyKmlLoadAndUpdateCoordinatesAndSave()
    {
        string fileName = "test.kml";
        CreateKml kml = new CreateKml("test", "unit test");
        KmlSerializerTextWriter kmlSerializerTextWriter = new KmlSerializerTextWriter(typeof(KmlModel.Kml));
        SaveKml saveKml = new SaveKml(kml, kmlSerializerTextWriter);
        saveKml.Execute(fileName);

        KmlModel.Kml? kmlToUpdate = kmlSerializerTextWriter.DoDeserialization(fileName);
        _updateKml.OldKml = kmlToUpdate;
        _updateKml.Placemark = new KmlModel.Placemark
        {
            LineString = new KmlModel.LineString
            {
                Coordinates = "12.8977216, 48.8408876, 2357"
            }
        };
        KmlModel.Kml? result = _updateKml.GenerateKml();
        Assert.IsTrue(result?.Document?.Placemarks?[0].LineString?.Coordinates?.Equals("12.8977216, 48.8408876, 2357"));

        KmlSerializerTextWriter kmlSerializer = new KmlSerializerTextWriter(typeof(KmlModel.Kml));
        kmlSerializer.DoSerialization(result, fileName);

        Assert.IsTrue(File.Exists(fileName));
    }
}