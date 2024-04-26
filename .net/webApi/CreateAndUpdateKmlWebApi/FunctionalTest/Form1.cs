using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace FunctionalTest;

public partial class Form1 : Form
{


    private static readonly HttpClient httpClient = new HttpClient();

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

                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(requestUri, content);

                log.AppendText(httpResponseMessage.StatusCode.ToString());
                log.AppendText(Environment.NewLine);
                log.AppendText("********************************");
                log.AppendText(Environment.NewLine);

                await Task.Delay(2000);
            }
        }
    }
}