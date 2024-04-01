using System.Globalization;
using CreateAndUpdateKmlLib;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using PreparePicturesAndHtmlAndUploadToWebsite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CreateAndUpdateKmlWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UpdateCoordinatesController : ControllerBase
{
    private readonly IUpdateKmlIfExistsOrCreateNewIfNot _updateKmlIfExistsOrCreateNewIfNot;

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
        string folder = GetValue(data, "folderName");
        folder = string.IsNullOrWhiteSpace(folder) ? "default" : folder;

        string fileName = GetValue(data, "fileName");
        fileName = string.IsNullOrWhiteSpace(fileName) ? "default" : fileName;

        string extension = ".kml";

        fileName = CommonStaticMethods.CreateFolderIfNotExistAndChangeFileExtenstion(folder, fileName, extension);

        if (string.IsNullOrWhiteSpace(data["lng"]?.ToString()) && string.IsNullOrWhiteSpace(data["lat"]?.ToString()))
        {
            throw new Exception("Coordinates cannot be empty!");
        }

        string coordinates = string.Join(',', data["lng"], data["lat"], "2357");
        //string coordinates = GetValue(data, "coordinates");
        CommonStaticMethods.WriteFileNameAndCoordinatesToJsonFile(coordinates, "test.json"); //ToDo: file test.json contains current location, which is defined in \html\script\config.js, if file name "test.json" gonna change, then config.js should be updated accordingly

        string strAbsolutePath = @"https://milosevtracking.azurewebsites.net";
        CommonStaticMethods.WriteConfigurationToJsonFile(strAbsolutePath, fileName, "config.json");

        _updateKmlIfExistsOrCreateNewIfNot.Execute(fileName, coordinates);
    }

    private string GetValue(JObject data, string value)
    {
        return (data[value] ?? string.Empty).Value<string>() ?? string.Empty;
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
            string folder = GetValue(data, "folderName");
            folder = string.IsNullOrWhiteSpace(folder) ? "default" : folder;

            string fileName = GetValue(data, "fileName");
            fileName = string.IsNullOrWhiteSpace(fileName) ? "default" : fileName;

            string host = GetValue(data, "host");
            string user = GetValue(data, "user");
            string pass = GetValue(data, "pass");

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
        string base64Image = data["image"]?.ToString() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(base64Image))
        {
            return BadRequest(new {message = "Error: No image." });
        }

        byte[] imageBytes = Convert.FromBase64String(base64Image);
        string folderName = data["folderName"]?.ToString() ?? "default";
        Directory.CreateDirectory(folderName);
        string imagePath = $"{folderName}\\{data["fileName"]}";
        await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);

        return Ok(new {message = "Image uploaded successfully." });
    }
}