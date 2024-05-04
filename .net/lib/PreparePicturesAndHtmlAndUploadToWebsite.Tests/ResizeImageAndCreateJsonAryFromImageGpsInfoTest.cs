using System.Diagnostics;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class ResizeImageAndCreateJsonAryFromImageGpsInfoTest
{

    [TestMethod]
    public void ResizeImageAndCreateJsonAryFromImageGpsInfoAndCheckIfFileExists()
    {
        ICreateJsonAryFromImageGpsInfo createJsonAryFromImageGpsInfo =
            new CreateJsonAryFromImageGpsInfo(new ExtractGpsInfoFromImage(), new UpdateJsonIfExistsOrCreateNewIfNot());
        IResizeImage resizeImage = new ResizeImage();

        ResizeImageAndCreateJsonAryFromImageGpsInfo resizeImageAndCreateJsonAryFromImageGpsInfo =
            new ResizeImageAndCreateJsonAryFromImageGpsInfo(resizeImage, createJsonAryFromImageGpsInfo);

        string rootFolder = @"C:\projects\web\googleMaps\customImageInsteadMarker";
        string jsonFileName = Path.Join(rootFolder, @"\test.json");
        if (File.Exists(jsonFileName))
        {
            File.Delete(jsonFileName);
        }
        string thumbsFolder = $@"{rootFolder}\thumbs";
        if (!Directory.Exists(thumbsFolder))
        {
            Directory.CreateDirectory(thumbsFolder);
        }

        string folderName = @"G:\slike";
        string[] directories = Directory.GetDirectories(folderName);

        string saveTo;
        string saveToFileName;
        string nameOfFileForJson;


        foreach (string directory in directories.Take(10))
        {
            foreach (string imageFileName in Directory.GetFiles(directory).Take(10))
            {
                try
                {
                    saveToFileName = 
                        $@"{Path.GetFileNameWithoutExtension(imageFileName)}thumb{Path.GetExtension(imageFileName)}";
                    saveTo = Path.Join(thumbsFolder, saveToFileName);

                    nameOfFileForJson = $"thumbs/{saveToFileName}";
                    resizeImageAndCreateJsonAryFromImageGpsInfo.Execute(imageFileName
                        , saveTo
                        , 25
                        , 25
                        , nameOfFileForJson
                        , jsonFileName);
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