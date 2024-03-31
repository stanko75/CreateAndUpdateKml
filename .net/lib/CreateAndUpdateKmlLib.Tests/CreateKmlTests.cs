//dotnet dotcover test --no-build --dcReportType=HTML

using Common;

namespace CreateAndUpdateKmlLib.Tests;

[TestClass]
public class GenerateKmlTests
{
    private readonly CreateKml _createKml;

    public GenerateKmlTests()
    {
        _createKml = new CreateKml("test", "unit test");
    }

    [TestMethod]
    public void CheckIfItsNotNull()
    {
        KmlModel.Kml result = _createKml.GenerateKml();
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void CreateWithCoordinates()
    {
        CommonMethodsForTests.Common.CreateKml(_createKml);
        Assert.AreEqual(_createKml.PlaceMarks?[0].LineString?.Coordinates, "12.8977216, 48.8408876, 2357, 12.8965296, 48.8417986, 2357, 12.8953309, 48.8426691, 2357");
    }
}