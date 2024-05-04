using System.Diagnostics;
using CreateAndUpdateKmlLib;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class PrepareTemplatesTests
{
    [TestMethod]
    public void ReplaceKeysInTemplateFilesWithProperValuesAndCheckIfFileAndFolderStructureMatchesExpected()
    {
        //string[] fileAndFolderStructure =
        //[
        //    @"kml\kml.kml",
        //    "pics",
        //    "thumbs",
        //    "www",
        //    @"www\css\index.css",
        //    @"www\lib\jquery-3.3.1.js",
        //    @"www\script\map.js",
        //    @"www\script\namespaces.js",
        //    @"www\script\pics2maps.js",
        //    @"www\script\thumbnails.js",
        //    @"www\index.html",
        //    @"www\joomlaPreview.html",
        //    @"www\/*albumName*/.js",
        //    @"www\/*albumName*/Thumbs.json"
        //];

        string[] expectedFileAndFolderStructure =
        [
            @"css\index.css",
            @"lib\jquery-3.3.1.js",
            @"script\map.js",
            @"script\namespaces.js",
            @"script\pics2maps.js",
            @"script\thumbnails.js",
            "index.html",
            "joomlaPreview.html"
        ];


        //Path.GetFullPath(@"..\..\..\..\..\..\html\blog\listOfFilesToReplace.json")

        string templateRootFolder = @"..\..\..\..\..\..\html\templateForBlog";

        string listOfFilesToReplaceJson = Path.Join(templateRootFolder, "listOfFilesToReplaceAndCopy.json");
        string listOfKeyValuesToReplaceInFilesJson = Path.Join(templateRootFolder, "listOfKeyValuesToReplaceInFiles.json");

        Assert.IsTrue(File.Exists(listOfFilesToReplaceJson), $"File: {listOfFilesToReplaceJson} does not exist!");
        Assert.IsTrue(File.Exists(listOfKeyValuesToReplaceInFilesJson), $"File: {listOfKeyValuesToReplaceInFilesJson} does not exist!");

        PrepareTemplates prepareTemplates = new PrepareTemplates();
        string saveToPath = @"..\..\..\..\..\..\html\blog\www";
        try
        {
            prepareTemplates.ReplaceKeysInTemplateFilesWithProperValues(listOfFilesToReplaceJson
                , listOfKeyValuesToReplaceInFilesJson
                , templateRootFolder
                , saveToPath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        foreach (string fileAndFolder in expectedFileAndFolderStructure)
        {
            string fileAndFolderTest = Path.Join(saveToPath, fileAndFolder);
            Assert.IsTrue(File.Exists(fileAndFolderTest),
                $"File: {Path.GetFullPath(fileAndFolderTest)} does not exists!");
        }
    }

}