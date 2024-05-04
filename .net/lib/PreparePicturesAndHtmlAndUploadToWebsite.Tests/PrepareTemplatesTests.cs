using System.Diagnostics;

namespace PreparePicturesAndHtmlAndUploadToWebsite.Tests;

[TestClass]
public class PrepareTemplatesTests
{
    [TestMethod]
    public void ReplaceKeysInTemplateFilesWithProperValuesAndCheckIfFileAndFolderStructureMatchesExpected()
    {
        string[] fileAndFolderStructure =
        [
            //@"kml\kml.kml",
            //"pics",
            //"thumbs",
            //"www",
            @"www\css\index.css",
            @"www\lib\jquery-3.3.1.js",
            @"www\script\map.js",
            @"www\script\namespaces.js",
            @"www\script\pics2maps.js",
            @"www\script\thumbnails.js",
            @"www\index.html",
            @"www\joomlaPreview.html",
            @"www\/*albumName*/.js",
            @"www\/*albumName*/Thumbs.json"
        ];

        //Path.GetFullPath(@"..\..\..\..\..\..\html\blog\listOfFilesToReplace.json")

        string listOfFilesToReplaceJson = @"..\..\..\..\..\..\html\blog\listOfFilesToReplaceAndCopy.json";
        string listOfKeyValuesToReplaceInFilesJson = @"..\..\..\..\..\..\html\blog\listOfKeyValuesToReplaceInFiles.json";

        Assert.IsTrue(File.Exists(listOfFilesToReplaceJson), $"File: {listOfFilesToReplaceJson} does not exist!");
        Assert.IsTrue(File.Exists(listOfKeyValuesToReplaceInFilesJson), $"File: {listOfKeyValuesToReplaceInFilesJson} does not exist!");

        PrepareTemplates prepareTemplates = new PrepareTemplates();
        try
        {
            prepareTemplates.ReplaceKeysInTemplateFilesWithProperValues(listOfFilesToReplaceJson
                , listOfKeyValuesToReplaceInFilesJson
                , @"..\..\..\..\..\..\html\blog"
                , @"prepareTemplates\www");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        foreach (string fileAndFolder in fileAndFolderStructure)
        {
            string fileAndFolderTest = Path.Join("prepareTemplates", fileAndFolder);
            Assert.IsTrue(File.Exists(fileAndFolderTest),
                $"File: {Path.GetFullPath(fileAndFolderTest)} does not exists!");
        }
    }

}