using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading;
using FunctionalTest.Log;

namespace FunctionalTest;

public class PostGpsPositionsFromFilesWithFileNameHandler(ILogger logger)
    : ICommandHandler<PostGpsPositionsFromFilesWithFileNameCommand>
{
    private ILogger _logger = logger;

    public void Execute(PostGpsPositionsFromFilesWithFileNameCommand command)
    {
        string addressText = command.AddressText;
        string gpsLocationsPath = command.GpsLocationsPath;
        CancellationToken cancellationToken = command.CancellationToken;

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
            }
        }
    }
}