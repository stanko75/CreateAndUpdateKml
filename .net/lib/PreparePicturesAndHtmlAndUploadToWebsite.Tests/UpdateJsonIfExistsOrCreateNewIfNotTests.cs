using System.Text.Json;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class UpdateJsonIfExistsOrCreateNewIfNotTests
{
    private const string FileName = "test.json";

    [TestMethod]
    public void CreateNewFile()
    {
        if (File.Exists(FileName))
        {
            File.Delete(FileName);
        }

        LatLngFileNameModel latLngFileNameModel = CreateModel(1, 2, "test1");

        UpdateJsonIfExistsOrCreateNewIfNot
            updateJsonIfExistsOrCreateNewIfNot = new UpdateJsonIfExistsOrCreateNewIfNot();
        updateJsonIfExistsOrCreateNewIfNot.Execute(FileName, latLngFileNameModel);

        Assert.IsTrue(File.Exists(FileName));
    }

    [TestMethod]
    public void UpdateExistingFile()
    {
        CreateNewFile();
        LatLngFileNameModel latLngFileNameModel = CreateModel(3, 4, "test2");

        UpdateJsonIfExistsOrCreateNewIfNot
            updateJsonIfExistsOrCreateNewIfNot = new UpdateJsonIfExistsOrCreateNewIfNot();
        updateJsonIfExistsOrCreateNewIfNot.Execute(FileName, latLngFileNameModel);

        string jsonString = File.ReadAllText(FileName);
        List<LatLngFileNameModel>? latLngFileNameModels = JsonSerializer.Deserialize<List<LatLngFileNameModel>>(jsonString);
        Assert.IsTrue(latLngFileNameModels is { Count: > 1 });
        Assert.IsTrue(latLngFileNameModels[0].fileName == "test1");
        Assert.IsTrue(latLngFileNameModels[1].fileName == "test2");
    }

    private LatLngFileNameModel CreateModel(double lat, double lng, string fileName)
    {
        LatLngFileNameModel latLngFileNameModel = new LatLngFileNameModel
        {
            lng = lng,
            lat = lat,
            fileName = fileName
        };

        return latLngFileNameModel;
    }
}