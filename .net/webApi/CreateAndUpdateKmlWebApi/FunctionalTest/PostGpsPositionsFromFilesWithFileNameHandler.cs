using Newtonsoft.Json.Linq;
using System.Text;
using FunctionalTest.Log;
using Microsoft.VisualBasic.Logging;

namespace FunctionalTest;

public class PostGpsPositionsFromFilesWithFileNameHandler(ILogger logger)
    : ICommandHandler<PostGpsPositionsFromFilesWithFileNameCommand>
{
    private ILogger _logger = logger;

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
                JObject myJObject = JObject.Parse(File.ReadAllText(file));
                jObjectKmlFileFolderLatLng["Longitude"] = myJObject["lng"];
                jObjectKmlFileFolderLatLng["Latitude"] = myJObject["lat"];

                string requestUri = Path.Combine(addressText, @"api/UpdateCoordinates/PostFileFolder");
                StringContent content = new StringContent($@"{jObjectKmlFileFolderLatLng}", Encoding.UTF8, "text/json");

                _logger.Log(new LogEntry(LoggingEventType.Information, $"Sending: {jObjectKmlFileFolderLatLng}"));

                HttpResponseMessage? httpResponseMessage = null;
                try
                {
                    httpResponseMessage = await httpClientPost.PostAsync(requestUri, content, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.Log(new LogEntry(LoggingEventType.Fatal, $"Exception: {ex.Message}", ex));
                }

                _logger.Log(new LogEntry(LoggingEventType.Fatal, $"{httpResponseMessage?.StatusCode.ToString()}"));
            }
        }
    }
}

//Task task = PostGpsPositionsFromFilesWithFileNameAsync(cancellationTokenSource.Token);