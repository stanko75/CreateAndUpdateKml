using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace FunctionalTest;

public partial class Form1 : Form
{


    private static readonly HttpClient HttpClientPost = new();
    private static readonly HttpClient HttpClientGet = new();

    public Form1()
    {
        InitializeComponent();
        if (File.Exists("jsconfigForTests.json"))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("jsconfigForTests.json")
                .Build();

            string? addressValue = configuration.GetSection("address").Value;
            if (address is not null) address.Text = addressValue;

            string? gpsLocationsPath = configuration.GetSection("gpsLocationsPath").Value;
            if (gpsLocationsPath is not null) tbGpsLocationsPath.Text = gpsLocationsPath;
        }
    }

    private void PostGpsPositionsFromFilesWithFileName_Click(object sender, EventArgs e)
    {
        Task task = PostGpsPositionsFromFilesWithFileNameMethod();

        log.AppendText(task.Status.ToString());
        log.AppendText(Environment.NewLine);
    }

    async Task PostGpsPositionsFromFilesWithFileNameMethod()
    {
        string addressText = address.Text;
        string gpsLocationsPath = tbGpsLocationsPath.Text;

        JObject jObjectKmlFileFolder = new JObject();
        jObjectKmlFileFolder["folderName"] = folderName.Text;
        jObjectKmlFileFolder["kmlFileName"] = kmlFileName.Text;

        if (string.IsNullOrWhiteSpace(kmlFileName.Text)) kmlFileName.Text = "default";
        if (string.IsNullOrWhiteSpace(folderName.Text)) folderName.Text = "default";

        if (Directory.Exists(gpsLocationsPath))
        {
            foreach (string file in Directory.GetFiles(gpsLocationsPath))
            {
                JObject myJObject = JObject.Parse(File.ReadAllText(file));
                //o["coordinates"] = $"{myJObject["lng"]}, {myJObject["lat"]}, 2357 ";
                jObjectKmlFileFolder["lng"] = myJObject["lng"];
                jObjectKmlFileFolder["lat"] = myJObject["lat"];

                string requestUri = Path.Combine(addressText, @"api/UpdateCoordinates/PostFileFolder");
                StringContent content = new StringContent($@"{jObjectKmlFileFolder}", Encoding.UTF8, "text/json");

                log.AppendText($"Sending: {jObjectKmlFileFolder}");
                log.AppendText(Environment.NewLine);
                log.AppendText(Environment.NewLine);

                HttpResponseMessage httpResponseMessage = await HttpClientPost.PostAsync(requestUri, content);

                log.AppendText(httpResponseMessage.StatusCode.ToString());
                log.AppendText(Environment.NewLine);
                log.AppendText("********************************");
                log.AppendText(Environment.NewLine);

                Uri baseUri = new Uri(addressText);
                Uri absoluteUri = new Uri(baseUri, "config.json");
                JObject configJson = null;

                try
                {
                    string configJsonString = await HttpClientGet.GetStringAsync(absoluteUri.AbsoluteUri);
                    configJson = JObject.Parse(configJsonString);
                }
                catch (Exception ex)
                {
                    log.AppendText("There is error with config.json:");
                    log.AppendText(Environment.NewLine);
                    log.AppendText(ex.Message);
                    log.AppendText(Environment.NewLine);
                }

                string? klmFileName = configJson?["KmlFileName"]?.ToString();
                string? currentLocation = configJson?["CurrentLocation"]?.ToString();
                //string? liveImageMarkersJsonUrl = configJson?["LiveImageMarkersJsonUrl"]?.ToString();

                CheckConfigJson(addressText, folderName.Text, klmFileName, kmlFileName.Text, "kml");
                CheckConfigJson(addressText, folderName.Text, currentLocation, "test", "json", true);

                await Task.Delay(2000);
            }
        }
    }

    private void CheckConfigJson(string addressText, string folderName, string? fileNameInConfigJsonOnWeb, string localFileName,
        string extension, bool checkInRoot = false)
    {
        UriBuilder uriBuilder = new UriBuilder(addressText);
        uriBuilder.Path = checkInRoot ? Path.ChangeExtension(localFileName, extension) : Path.Combine(folderName, Path.ChangeExtension(localFileName, extension));
        Uri fileNameUri = uriBuilder.Uri;
        if (!string.Equals(fileNameInConfigJsonOnWeb, fileNameUri.AbsoluteUri,
                StringComparison.InvariantCultureIgnoreCase))
        {
            string message = $"There is an error in config.json! {fileNameInConfigJsonOnWeb} is not equal {fileNameUri.AbsoluteUri}!";
            log.AppendText(message);
            log.AppendText(Environment.NewLine);
            throw new Exception(message);
        }
    }
}