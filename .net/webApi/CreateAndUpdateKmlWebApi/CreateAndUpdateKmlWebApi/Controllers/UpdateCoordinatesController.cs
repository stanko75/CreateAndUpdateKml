using System.Globalization;
using CreateAndUpdateKmlLib;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using PreparePicturesAndHtmlAndUploadToWebsite;
using CreateAndUpdateKmlWebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreateAndUpdateKmlWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UpdateCoordinatesController : ControllerBase
{
    private readonly IUpdateKmlIfExistsOrCreateNewIfNot _updateKmlIfExistsOrCreateNewIfNot;
    private const string ConfigFileName = "config.json";

    private const string CurrentLocation = "test.json";

    private const string RootUrl = "https://milosevtracking.azurewebsites.net";
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

        if (string.IsNullOrWhiteSpace(data["lng"]?.ToString()) && string.IsNullOrWhiteSpace(data["lat"]?.ToString()))
        {
            throw new Exception("Coordinates cannot be empty!");
        }

        string coordinates = string.Join(',', data["lng"], data["lat"], "2357");
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

            string fileName = CommonStaticMethods.GetValue(data, "kmlFileName");
            fileName = string.IsNullOrWhiteSpace(fileName) ? "default" : fileName;

            string host = CommonStaticMethods.GetValue(data, "host");
            string user = CommonStaticMethods.GetValue(data, "user");
            string pass = CommonStaticMethods.GetValue(data, "pass");

            string extension = ".kml";
            fileName = CommonStaticMethods.ChangeFileExtension(fileName, extension);

            string remoteRootFolder = "/allWithPics/travelBuddies";

            PrepareHtmlFilesAndUpload prepareHmlFilesAndUpload = new PrepareHtmlFilesAndUpload(new PrepareHtmlFiles()
                , new MirrorDirAndFileStructureOnFtp(new FtpUpload(host, user, pass))
                , new WriteConfigurationToJsonFile());
            prepareHmlFilesAndUpload.Execute(@"html\blog"
                , "prepareForUpload"
                , folder
                , fileName
                , remoteRootFolder);

            return Ok(@$"Uploaded: {remoteRootFolder}/{folder}/{fileName}");
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
            ImageModel imageModel = new ImageModel(kmlFileFolderModel, data, RootUrl);
            Directory.CreateDirectory(kmlFileFolderModel.FolderName);

            await System.IO.File.WriteAllBytesAsync(imageModel.ImageOriginalFileName, imageModel.ImageBytes);

            ICreateJsonAryFromImageGpsInfo createJsonAryFromImageGpsInfo =
                new CreateJsonAryFromImageGpsInfo(new ExtractGpsInfoFromImage(), new UpdateJsonIfExistsOrCreateNewIfNot());
            IResizeImage resizeImage = new ResizeImage();

            ResizeImageAndCreateJsonAryFromImageGpsInfo resizeImageAndCreateJsonAryFromImageGpsInfo =
                new ResizeImageAndCreateJsonAryFromImageGpsInfo(resizeImage, createJsonAryFromImageGpsInfo);
            //
            resizeImageAndCreateJsonAryFromImageGpsInfo.Execute(imageModel.ImageOriginalFileName
                , imageModel.ImageThumbsFileName
                , 25
                , 25
                , imageModel.NameOfFileForJson
                , imageModel.JsonFileName);

            return Ok(new
            {
                message =
                    $"Image uploaded to {Path.GetFullPath(imageModel.ImageOriginalFileName)}" +
                    $"{Environment.NewLine}" +
                    $"***" +
                    $"{Environment.NewLine}" +

                    $"JSON file saved in {Path.GetFullPath(imageModel.JsonFileName)}" +
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