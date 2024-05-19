using System.Globalization;
using CreateAndUpdateKmlLib;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using PreparePicturesAndHtmlAndUploadToWebsite;
using CreateAndUpdateKmlWebApi.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreateAndUpdateKmlWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UpdateCoordinatesController : ControllerBase
{
    private readonly IUpdateKmlIfExistsOrCreateNewIfNot _updateKmlIfExistsOrCreateNewIfNot;
    private const string ConfigFileName = "config.json";

    private const string CurrentLocation = "test.json";

    //private const string RootUrl = "https://milosevtracking.azurewebsites.net";
    private const string RootUrl = "https://localhost:7293/";
    //private const string RootUrl =
    //    "http://livetracking.milosev.com:100/.net/webApi/CreateAndUpdateKmlWebApi/CreateAndUpdateKmlWebApi";

    public UpdateCoordinatesController(IUpdateKmlIfExistsOrCreateNewIfNot updateKmlIfExistsOrCreateNewIfNot)
    {
        _updateKmlIfExistsOrCreateNewIfNot = updateKmlIfExistsOrCreateNewIfNot;
    }

    // GET: api/<UpdateCoordinatesController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<UpdateCoordinatesController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<UpdateCoordinatesController>
    [HttpPost]
    [Route("PostFileFolder")]
    public void PostFileFolder([FromBody] JObject data)
    {
        string extension = ".kml";

        KmlFileFolderModel kmlFileFolderModel = new KmlFileFolderModel(data);
        CommonStaticMethods.WriteConfigurationToJsonFile(kmlFileFolderModel.FolderName
            , kmlFileFolderModel.KmlFileName
            , CurrentLocation
            , ConfigFileName
            , RootUrl);

        kmlFileFolderModel.KmlFileName = CommonStaticMethods.CreateFolderIfNotExistAndChangeFileExtenstion(kmlFileFolderModel.FolderName, kmlFileFolderModel.KmlFileName, extension);

        if (string.IsNullOrWhiteSpace(data["Longitude"]?.ToString()) && string.IsNullOrWhiteSpace(data["Latitude"]?.ToString()))
        {
            throw new Exception("Coordinates cannot be empty!");
        }

        string coordinates = string.Join(',', data["Longitude"], data["Latitude"], "2357");
        CommonStaticMethods.WriteFileNameAndCoordinatesToJsonFile(coordinates,
            CurrentLocation);

        _updateKmlIfExistsOrCreateNewIfNot.Execute(kmlFileFolderModel.KmlFileName, coordinates);
    }

    // POST api/<UpdateCoordinatesController>
    [HttpPost]
    public void Post([FromBody] string coordinates)
    {
        string[] aryCoordinates = coordinates.Split(',');

        if (aryCoordinates.Length > 1)
        {
            var objCoordinates = new
            {
                lat = Convert.ToDouble(aryCoordinates[1].Trim(), new NumberFormatInfo { NumberDecimalSeparator = "." }),
                lng = Convert.ToDouble(aryCoordinates[0].Trim(), new NumberFormatInfo { NumberDecimalSeparator = "." })
            };
            string jsonCoordinates = JsonSerializer.Serialize(objCoordinates);
            System.IO.File.WriteAllText("test.json", jsonCoordinates);

            _updateKmlIfExistsOrCreateNewIfNot.Execute("test.kml", coordinates);
        }
    }

    // POST api/<UploadToBlogOldController>
    [HttpPost]
    [Route("UploadToBlog")]
    public IActionResult UploadToBlog([FromBody] JObject data)
    {
        try
        {
            string folder = CommonStaticMethods.GetValue(data, "folderName");
            folder = string.IsNullOrWhiteSpace(folder) ? "default" : folder;

            string kmlFileName = CommonStaticMethods.GetValue(data, "kmlFileName");
            kmlFileName = string.IsNullOrWhiteSpace(kmlFileName) ? "default" : kmlFileName;

            string host = CommonStaticMethods.GetValue(data, "host");
            string user = CommonStaticMethods.GetValue(data, "user");
            string pass = CommonStaticMethods.GetValue(data, "pass");

            string extension = ".kml";
            kmlFileName = CommonStaticMethods.ChangeFileExtension(kmlFileName, extension);

            string remoteRootFolder = "/allWithPics/travelBuddies";

            ICopyHtmlFilesAndUpload copyHtmlFilesAndUpload = new CopyHtmlFilesAndUpload(new CopyHtmlFiles()
                , new MirrorDirAndFileStructureOnFtp(new FtpUpload(host, user, pass))
                , new WriteConfigurationToJsonFile());

            IPrepareTemplates prepareTemplates = new PrepareTemplates();
            PrepareCopyAndUploadHtmlFiles prepareCopyAndUploadHtmlFiles =
                new PrepareCopyAndUploadHtmlFiles(copyHtmlFilesAndUpload, prepareTemplates);
            
            string listOfKeyValuesToReplaceInFilesJson = System.IO.File.ReadAllText(@"html\templateForBlog\listOfKeyValuesToReplaceInFiles.json");
            Dictionary<string, string>? listOfKeyValuesToReplaceInFilesDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(listOfKeyValuesToReplaceInFilesJson);

            if (listOfKeyValuesToReplaceInFilesDict is not null)
            {
                listOfKeyValuesToReplaceInFilesDict["/*galleryName*/"] = folder;
                listOfKeyValuesToReplaceInFilesDict["/*gapikey*/"] = "AIzaSyCC96EGKbYxxIadYOGTozj8NdD0F5CIYM4";
                listOfKeyValuesToReplaceInFilesDict["/*ogTitle*/"] = "ogTitle";
                listOfKeyValuesToReplaceInFilesDict["/*ogDescription*/"] = "ogDescription";
                listOfKeyValuesToReplaceInFilesDict["/*ogImage*/"] = "ogImage";
                listOfKeyValuesToReplaceInFilesDict["/*ogUrl*/"] = "ogUrl";
                listOfKeyValuesToReplaceInFilesDict["/*picsJson*/"] = Path.GetFileNameWithoutExtension(kmlFileName);
                listOfKeyValuesToReplaceInFilesDict["/*zoom*/"] = "4";
                listOfKeyValuesToReplaceInFilesDict["/*joomlaThumbsPath*/"] = "joomlaThumbsPath";
                listOfKeyValuesToReplaceInFilesDict["/*joomlaImgSrcPath*/"] = "joomlaImgSrcPath";
            }

            System.IO.File.WriteAllText("listOfKeyValuesToReplaceInFiles.json", JsonConvert.SerializeObject(listOfKeyValuesToReplaceInFilesDict));

            prepareCopyAndUploadHtmlFiles.Execute(@"html\templateForBlog\listOfFilesToReplaceAndCopy.json"
                //, @"html\templateForBlog\listOfKeyValuesToReplaceInFiles.json"
                , @"listOfKeyValuesToReplaceInFiles.json"
                , @"html\templateForBlog"
                , @"html\blog\www"
                , @"html\blog\www"
                , @"prepareForUpload"
                , folder
                , Path.Join(folder, kmlFileName)
                , remoteRootFolder);

            /*
            copyHtmlFilesAndUpload.Execute(@"html\blog"
                , "prepareForUpload"
                , folder
                , fileName
                , remoteRootFolder);
            */

            return Ok(@$"Uploaded: {remoteRootFolder}/{folder}/{kmlFileName}");
        }
        catch (Exception e)
        {
            return BadRequest($"Exception message: {e.Message}, inner exception: {e.InnerException}");
        }
    }

    [HttpPost]
    [Route("UploadImage")]

    public async Task<IActionResult> UploadImage([FromBody] JObject data)
    {
        try
        {
            KmlFileFolderModel kmlFileFolderModel = new KmlFileFolderModel(data);
            CommonStaticMethods.WriteConfigurationToJsonFile(kmlFileFolderModel.FolderName
                , kmlFileFolderModel.KmlFileName
                , CurrentLocation
                , ConfigFileName
                , RootUrl);
            ImageModel imageModel = new ImageModel(kmlFileFolderModel, data,@"..\..\");
            Directory.CreateDirectory(kmlFileFolderModel.FolderName);

            await System.IO.File.WriteAllBytesAsync(imageModel.ImageOriginalFileName, imageModel.ImageBytes);

            IFillLatLngFileNameModelFromImageGpsInfo fillLatLngFileNameModelFromImageGpsInfo =
                new FillLatLngFileNameModelFromImageGpsInfo(new ExtractGpsInfoFromImage());
            IResizeImage resizeImage = new ResizeImage();

            ResizeImageAndCreateJsonAryFromImageGpsInfo resizeImageAndCreateJsonAryFromImageGpsInfo =
                new ResizeImageAndCreateJsonAryFromImageGpsInfo(resizeImage, fillLatLngFileNameModelFromImageGpsInfo);

            if (!System.IO.File.Exists(imageModel.ImageOriginalFileName))
            {
                throw new Exception($"File: {Path.GetFullPath(imageModel.ImageOriginalFileName)} does not exist!");
            }

            LatLngFileNameModel? latLngFileNameModel = resizeImageAndCreateJsonAryFromImageGpsInfo.Execute(imageModel.ImageOriginalFileName
                , imageModel.ImageThumbsFileName
                , 200
                , 200
                , imageModel.NameOfFileForJson);

            IUpdateJsonIfExistsOrCreateNewIfNot updateJsonIfExistsOrCreateNewIfNot =
                new UpdateJsonIfExistsOrCreateNewIfNot();

            if (latLngFileNameModel is not null)
            {
                updateJsonIfExistsOrCreateNewIfNot.Execute(imageModel.FileNameThumbsJson, latLngFileNameModel);
                updateJsonIfExistsOrCreateNewIfNot.Execute(imageModel.FileNameJson, latLngFileNameModel);
            }

            return Ok(new
            {
                message =
                    $"Image uploaded to {Path.GetFullPath(imageModel.ImageOriginalFileName)}" +
                    $"{Environment.NewLine}" +
                    $"***" +
                    $"{Environment.NewLine}" +

                    $"JSON file saved in {Path.GetFullPath(imageModel.FileNameThumbsJson)}" +
                    $"{Environment.NewLine}" +
                    $"***" +

                    $"{Environment.NewLine}" +
                    $"ImageThumbsFileName file saved in {Path.GetFullPath(imageModel.ImageThumbsFileName)}"
            });
        }
        catch (Exception e)
        {
            return BadRequest($"Exception message: {e.Message}, inner exception: {e.InnerException}");
        }
    }
}