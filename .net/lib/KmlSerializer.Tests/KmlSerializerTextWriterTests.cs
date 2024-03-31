using Common;
using CreateAndUpdateKmlLib;

namespace KmlSerializer.Tests;

[TestClass]
public class KmlSerializerTextWriterTests
{
    private readonly CreateKml _createKml;

    public KmlSerializerTextWriterTests()
    {
        _createKml = new CreateKml("test", "unit test");
    }

    [TestMethod]
    public void CheckIfItIsSavedToFile()
    {
        CommonMethodsForTests.Common.CreateKml(_createKml);
        KmlModel.Kml result = _createKml.GenerateKml();
        string fileName = "testhost.kml";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        KmlSerializerTextWriter kmlSerializerTextWriter = new KmlSerializerTextWriter(typeof(KmlModel.Kml));
        kmlSerializerTextWriter.DoSerialization(result, fileName);
        Assert.IsTrue(File.Exists(fileName));
    }
}