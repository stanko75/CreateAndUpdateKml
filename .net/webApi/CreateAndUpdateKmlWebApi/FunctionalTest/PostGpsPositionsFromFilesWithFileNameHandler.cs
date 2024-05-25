using Newtonsoft.Json.Linq;
using System.Text;
using FunctionalTest.Log;

namespace FunctionalTest;

public class PostGpsPositionsFromFilesWithFileNameHandler(ILogger logger)
    : ICommandHandler<PostGpsPositionsFromFilesWithFileNameCommand>
{
    public async Task Execute(PostGpsPositionsFromFilesWithFileNameCommand command)
    {
        string addressText = command.AddressText;
        string gpsLocationsPath = command.GpsLocationsPath;
        CancellationToken cancellationToken = command.CancellationToken;
        HttpClient httpClientPost = command.HttpClientPost;

        JObject jObjectKmlFileFolderLatLng = new JObject
        {
            ["folderName"] = command.FolderName,
            ["kmlFileName"] = command.KmlFileName
        };

        if (Directory.Exists(gpsLocationsPath))
        {
            foreach (string file in Directory.GetFiles(gpsLocationsPath))
            {
                cancellationToken.ThrowIfCancellationRequested();
                JObject myJObject = JObject.Parse(await File.ReadAllTextAsync(file, cancellationToken));
                jObjectKmlFileFolderLatLng["Longitude"] = myJObject["lng"];
                jObjectKmlFileFolderLatLng["Latitude"] = myJObject["lat"];

                string requestUri = Path.Combine(addressText, @"api/UpdateCoordinates/PostFileFolder");
                StringContent content = new StringContent($@"{jObjectKmlFileFolderLatLng}", Encoding.UTF8, "text/json");

                logger.Log($"Sending: {jObjectKmlFileFolderLatLng}");

                HttpResponseMessage? httpResponseMessage = null;
                try
                {
                    httpResponseMessage = await httpClientPost.PostAsync(requestUri, content, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.Log(new Exception($"Exception: {ex.Message}", ex));
                }

                logger.Log($"{httpResponseMessage?.StatusCode.ToString()}");

                JObject configJson =
                    await StaticCommon.GetConfigJson(StaticCommon.GetConfigJsonUri(addressText).AbsoluteUri,
                        httpClientPost, logger);

                string? klmFileName = configJson["KmlFileName"]?.ToString();
                string? currentLocation = configJson["CurrentLocation"]?.ToString();
                //string? liveImageMarkersJsonUrl = configJson?["LiveImageMarkersJsonUrl"]?.ToString();

                UriBuilder kmlUri = StaticCommon.CheckConfigJson(addressText, command.FolderName, klmFileName,
                    command.KmlFileName, "kml", logger);
                UriBuilder testJsonUri =
                    StaticCommon.CheckConfigJson(addressText, command.FolderName, currentLocation, "test", "json",
                        logger, true);
                try
                {
                    string kmlFileString =
                        await httpClientPost.GetStringAsync(kmlUri.Uri.AbsoluteUri, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.Log(new Exception("There is error with kmlFile:" + kmlUri.Uri.AbsoluteUri));
                    throw new Exception(ex.Message);
                }

                try
                {
                    string testJsonString =
                        await httpClientPost.GetStringAsync(testJsonUri.Uri.AbsoluteUri, cancellationToken);
                }
                catch (Exception ex)
                {
                    logger.Log(new Exception("There is error with test.json:" + testJsonUri.Uri.AbsoluteUri));
                    throw new Exception(ex.Message);
                }

                await Task.Delay(2000, cancellationToken);

            }
        }
    }
}