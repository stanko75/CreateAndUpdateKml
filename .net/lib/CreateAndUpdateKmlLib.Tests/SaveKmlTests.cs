using Common;
using KmlSerializer;

namespace CreateAndUpdateKmlLib.Tests;

[TestClass]
public class SaveKmlTests
{
    private readonly SaveKml _saveKml;
    private readonly string _fileName = "testhost.kml";

    public SaveKmlTests()
    {
        CreateKml kmlGenerator = new CreateKml("test", "test");
        CommonMethodsForTests.Common.CreateKml(kmlGenerator);
        _saveKml = new SaveKml(kmlGenerator, new KmlSerializerTextWriter(typeof(KmlModel.Kml)));
    }

    [TestMethod]
    public void CheckIfItIsSavedToFile()
    {
        if (File.Exists(_fileName))
        {
            File.Delete(_fileName);
        }

        _saveKml.Execute(_fileName);
        Assert.IsTrue(File.Exists(_fileName));
    }
}