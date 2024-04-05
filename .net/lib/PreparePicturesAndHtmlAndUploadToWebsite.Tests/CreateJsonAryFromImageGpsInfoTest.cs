
using System.Diagnostics;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class CreateJsonAryFromImageGpsInfoTest
{

    [TestMethod]
    public void CreateJsonAryFromImagesInFolderAndCheckIfFileExists()
    {
        CreateJsonAryFromImageGpsInfo createJsonAryFromImageGpsInfo =
            new CreateJsonAryFromImageGpsInfo(new ExtractGpsInfoFromImage(), new UpdateJsonIfExistsOrCreateNewIfNot());

        string jsonFileName = "createJsonAryFromImagesInFolder.json";
        if (File.Exists(jsonFileName))
        {
            File.Delete(jsonFileName);
        }

        string folderName = @"G:\slike";
        string[] directories = Directory.GetDirectories(folderName);

        foreach (string directory in directories.Take(10))
        {
            foreach (string imageFileName in Directory.GetFiles(directory).Take(10))
            {
                try
                {
                    createJsonAryFromImageGpsInfo.Execute(imageFileName, imageFileName, jsonFileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        Assert.IsTrue(File.Exists(jsonFileName));
    }
}