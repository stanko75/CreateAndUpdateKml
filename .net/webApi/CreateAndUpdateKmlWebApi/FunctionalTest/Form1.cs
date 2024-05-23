using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FunctionalTest.Log;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FunctionalTest;

public partial class Form1 : Form
{
    CancellationTokenSource? cancellationTokenSource = new CancellationTokenSource();
    private bool cancellationTokenSourceDisposed = false;
    private static readonly HttpClient HttpClientPost = new();
    private static readonly HttpClient HttpClientGet = new();

    private TextBoxLogger _textBoxLogger;

    private const string jsconfigForTests = "jsconfigForTests.json";

    public Form1()
    {
        InitializeComponent();
        _textBoxLogger = new TextBoxLogger(log);
        if (File.Exists(jsconfigForTests))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(jsconfigForTests)
                .Build();

            string? addressValue = configuration.GetSection("address").Value;
            if (address is not null) address.Text = addressValue;

            string? gpsLocationsPath = configuration.GetSection("gpsLocationsPath").Value;
            if (gpsLocationsPath is not null) tbGpsLocationsPath.Text = gpsLocationsPath;

            string? ftpUser = configuration.GetSection("ftpUser").Value;
            if (ftpUser is not null) tbFtpUser.Text = ftpUser;

            string? ftpHost = configuration.GetSection("ftpHost").Value;
            if (ftpHost is not null) tbFtpHost.Text = ftpHost;

            string? ftpPass = configuration.GetSection("ftpPass").Value;
            if (ftpPass is not null) tbFtpPass.Text = ftpPass;

            string? strImagesPath = configuration.GetSection("imagesPath").Value;
            if (strImagesPath is not null) imagesPath.Text = strImagesPath;

            string? strKmlFileName = configuration.GetSection("kmlFileName").Value;
            if (strKmlFileName is not null) kmlFileName.Text = strKmlFileName;

            string? strFolderName = configuration.GetSection("folderName").Value;
            if (strFolderName is not null) folderName.Text = strFolderName;
        }
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        JObject jsonConfig = new JObject();

        jsonConfig["address"] = address.Text;
        jsonConfig["gpsLocationsPath"] = tbGpsLocationsPath.Text;
        jsonConfig["ftpUser"] = tbFtpUser.Text;
        jsonConfig["ftpHost"] = tbFtpHost.Text;
        jsonConfig["ftpPass"] = tbFtpPass.Text;

        jsonConfig["folderName"] = folderName.Text;
        jsonConfig["kmlFileName"] = kmlFileName.Text;
        jsonConfig["imagesPath"] = imagesPath.Text;

        string json = jsonConfig.ToString(Formatting.Indented);
        File.WriteAllText(@$"..\..\..\{jsconfigForTests}", json);
    }

    private void PostGpsPositionsFromFilesWithFileName_Click(object sender, EventArgs e)
    {
        if (cancellationTokenSourceDisposed)
        {
            cancellationTokenSource = new CancellationTokenSource();
        }

        if (cancellationTokenSource != null)
        {
            var command = new PostGpsPositionsFromFilesWithFileNameCommand
            {
                AddressText = address.Text,
                KmlFileName = kmlFileName.Text,
                FolderName = folderName.Text,
                HttpClientPost = HttpClientPost,
                GpsLocationsPath = tbGpsLocationsPath.Text,
                CancellationToken = cancellationTokenSource.Token
            };

            ICommandHandler<PostGpsPositionsFromFilesWithFileNameCommand> commandE =
                new PostGpsPositionsFromFilesWithFileNameHandler(new TextBoxLogger(log));

            Task task = commandE.Execute(command);
            log.AppendText(task.Status.ToString());
        }

        log.AppendText(Environment.NewLine);
    }

    async Task PostGpsPositionsFromFilesWithFileNameAsync(CancellationToken cancellationToken)
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
                cancellationToken.ThrowIfCancellationRequested();
                JObject myJObject = JObject.Parse(File.ReadAllText(file));
                //o["coordinates"] = $"{myJObject["lng"]}, {myJObject["lat"]}, 2357 ";
                jObjectKmlFileFolder["Longitude"] = myJObject["lng"];
                jObjectKmlFileFolder["Latitude"] = myJObject["lat"];

                string requestUri = Path.Combine(addressText, @"api/UpdateCoordinates/PostFileFolder");
                StringContent content = new StringContent($@"{jObjectKmlFileFolder}", Encoding.UTF8, "text/json");

                log.AppendText($"Sending: {jObjectKmlFileFolder}");
                log.AppendText(Environment.NewLine);
                log.AppendText(Environment.NewLine);

                HttpResponseMessage? httpResponseMessage = null;
                try
                {
                    httpResponseMessage = await HttpClientPost.PostAsync(requestUri, content, cancellationToken);
                }
                catch (Exception ex)
                {
                    log.AppendText("There is error with:" + requestUri);
                    log.AppendText(Environment.NewLine);
                    log.AppendText(ex.Message);
                    log.AppendText(Environment.NewLine);
                }

                log.AppendText(httpResponseMessage?.StatusCode.ToString());
                log.AppendText(Environment.NewLine);
                log.AppendText("********************************");
                log.AppendText(Environment.NewLine);

                JObject configJson = await GetConfigJson(GetConfigJsonUri(addressText).AbsoluteUri);

                string? klmFileName = configJson?["KmlFileName"]?.ToString();
                string? currentLocation = configJson?["CurrentLocation"]?.ToString();
                //string? liveImageMarkersJsonUrl = configJson?["LiveImageMarkersJsonUrl"]?.ToString();

                UriBuilder kmlUri = CheckConfigJson(addressText, folderName.Text, klmFileName, kmlFileName.Text, "kml");
                UriBuilder testJsonUri =
                    CheckConfigJson(addressText, folderName.Text, currentLocation, "test", "json", true);

                try
                {
                    string kmlFileString = await HttpClientGet.GetStringAsync(kmlUri.Uri.AbsoluteUri, cancellationToken);
                }
                catch (Exception ex)
                {
                    log.AppendText("There is error with kmlFile:" + kmlUri.Uri.AbsoluteUri);
                    log.AppendText(Environment.NewLine);
                    log.AppendText(ex.Message);
                    log.AppendText(Environment.NewLine);
                    throw new Exception(ex.Message);
                }

                try
                {
                    string testJsonString = await HttpClientGet.GetStringAsync(testJsonUri.Uri.AbsoluteUri);
                }
                catch (Exception ex)
                {
                    log.AppendText("There is error with test.json:" + testJsonUri.Uri.AbsoluteUri);
                    log.AppendText(Environment.NewLine);
                    log.AppendText(ex.Message);
                    log.AppendText(Environment.NewLine);
                    throw new Exception(ex.Message);
                }

                await Task.Delay(2000);
            }
        }
    }

    private Uri GetConfigJsonUri(string addressText)
    {
        Uri baseUri = new Uri(addressText);
        return new Uri(baseUri, "config.json");
    }

    private async Task<JObject> GetConfigJson(string uri)
    {
        try
        {
            string configJsonString = await HttpClientGet.GetStringAsync(uri);
            return JObject.Parse(configJsonString);
        }
        catch (Exception ex)
        {
            log.AppendText("There is error with config.json:" + uri);
            log.AppendText(Environment.NewLine);
            log.AppendText(ex.Message);
            log.AppendText(Environment.NewLine);
            throw new Exception(ex.Message);
        }
    }

    private UriBuilder CheckConfigJson(string addressText, string folderNameString, string? fileNameInConfigJsonOnWeb,
        string localFileName,
        string extension, bool checkInRoot = false)
    {
        UriBuilder uriBuilder = new UriBuilder(addressText);
        uriBuilder.Path = checkInRoot
            ? Path.ChangeExtension(localFileName, extension)
            : Path.Combine(folderNameString, Path.ChangeExtension(localFileName, extension));
        Uri fileNameUri = uriBuilder.Uri;
        if (!string.Equals(fileNameInConfigJsonOnWeb, fileNameUri.AbsoluteUri,
                StringComparison.InvariantCultureIgnoreCase))
        {
            string message =
                $"There is an error in config.json! {fileNameInConfigJsonOnWeb} is not equal {fileNameUri.AbsoluteUri}!";
            log.AppendText(message);
            log.AppendText(Environment.NewLine);
            throw new Exception(message);
        }

        return uriBuilder;
    }

    private void UploadImage_Click(object sender, EventArgs e)
    {
        if (cancellationTokenSourceDisposed)
        {
            cancellationTokenSource = new CancellationTokenSource();
        }
        Task task = UploadImageAsync(cancellationTokenSource.Token);

        log.AppendText(task.Status.ToString());
        log.AppendText(Environment.NewLine);
    }

    async Task UploadImageAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(kmlFileName.Text)) kmlFileName.Text = "default";
        if (string.IsNullOrWhiteSpace(folderName.Text)) folderName.Text = "default";

        string addressText = address.Text;
        string imagesPathString = imagesPath.Text;
        if (Directory.Exists(imagesPathString))
        {
            foreach (string imageFile in Directory.GetFiles(imagesPathString))
            {
                cancellationToken.ThrowIfCancellationRequested();
                string base64Image = ConvertImageToBase64(imageFile);

                JObject jObjectKmlFileFolder = new JObject();
                jObjectKmlFileFolder["folderName"] = folderName.Text;
                jObjectKmlFileFolder["kmlFileName"] = kmlFileName.Text;
                jObjectKmlFileFolder["base64Image"] = base64Image;
                jObjectKmlFileFolder["imageFileName"] = Path.GetFileName(imageFile);

                string jsonContent = jObjectKmlFileFolder.ToString();
                StringContent content = new StringContent(jsonContent);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string requestUri = Path.Combine(addressText, @"api/UpdateCoordinates/UploadImage");
                log.AppendText("Sending");
                log.AppendText(Environment.NewLine);
                log.AppendText(Environment.NewLine);

                try
                {
                    HttpResponseMessage httpResponseMessage = await HttpClientPost.PostAsync(requestUri, content);
                    log.AppendText(httpResponseMessage.StatusCode.ToString());
                    log.AppendText(Environment.NewLine);
                    if (!httpResponseMessage.IsSuccessStatusCode)
                    {
                        string errorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
                        log.AppendText(Environment.NewLine);
                        log.AppendText(errorMessage);
                        log.AppendText(Environment.NewLine);
                        throw new Exception(errorMessage);
                    }

                    string message = await httpResponseMessage.Content.ReadAsStringAsync();
                    log.AppendText(Environment.NewLine);
                    log.AppendText(message);
                    log.AppendText(Environment.NewLine);
                    log.AppendText("********************************");
                    log.AppendText(Environment.NewLine);
                }
                catch (Exception ex)
                {
                    log.AppendText(ex.Message);
                    log.AppendText(Environment.NewLine);
                    log.AppendText("********************************");
                    log.AppendText(Environment.NewLine);
                    throw new Exception(ex.Message);
                }

                JObject configJson = await GetConfigJson(GetConfigJsonUri(addressText).AbsoluteUri);
                string liveImageMarkersJson = configJson["LiveImageMarkersJsonUrl"]?.ToString();

                UriBuilder liveImageMarkersJsonUri = CheckConfigJson(addressText, folderName.Text, liveImageMarkersJson, $"{kmlFileName.Text}Thumbs", "json");
                string liveImageMarkersJsonstring;
                try
                {
                    liveImageMarkersJsonstring = await HttpClientGet.GetStringAsync(liveImageMarkersJsonUri.Uri.AbsoluteUri);
                }
                catch (Exception ex)
                {
                    log.AppendText("There is error with thumbs file: " + liveImageMarkersJsonUri.Uri.AbsoluteUri);
                    log.AppendText(Environment.NewLine);
                    log.AppendText(ex.Message);
                    log.AppendText(Environment.NewLine);
                    throw new Exception(ex.Message);
                }

                if (!liveImageMarkersJsonstring.Contains(Path.GetFileName(imageFile)))
                {
                    string message = $"{Path.GetFileName(imageFile)} is not included in thumbs!";
                    log.AppendText(message);
                    log.AppendText(Environment.NewLine);
                    throw new Exception(message);
                }

            }
        }
    }

    static string ConvertImageToBase64(string imagePath)
    {
        byte[] imageBytes = File.ReadAllBytes(imagePath);
        return Convert.ToBase64String(imageBytes);
    }

    private void UploadToBlog_Click(object sender, EventArgs e)
    {
        if (cancellationTokenSourceDisposed)
        {
            cancellationTokenSource = new CancellationTokenSource();
        }
        Task task = UploadToBlogAsync(cancellationTokenSource.Token);

        log.AppendText(task.Status.ToString());
        log.AppendText(Environment.NewLine);
    }

    async Task UploadToBlogAsync(CancellationToken cancellationToken)
    {
        string addressText = address.Text;

        JObject jObjectKmlFileFolder = new JObject();
        jObjectKmlFileFolder["folderName"] = folderName.Text;
        jObjectKmlFileFolder["kmlFileName"] = kmlFileName.Text;
        jObjectKmlFileFolder["host"] = tbFtpHost.Text;
        jObjectKmlFileFolder["user"] = tbFtpUser.Text;
        jObjectKmlFileFolder["pass"] = tbFtpPass.Text;

        string jsonContent = jObjectKmlFileFolder.ToString();
        StringContent content = new StringContent(jsonContent);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        string requestUri = Path.Combine(addressText, @"api/UpdateCoordinates/UploadToBlog");
        log.AppendText("Sending");
        log.AppendText(Environment.NewLine);
        log.AppendText(Environment.NewLine);

        try
        {
            HttpResponseMessage httpResponseMessage = await HttpClientPost.PostAsync(requestUri, content);
            log.AppendText(httpResponseMessage.StatusCode.ToString());
            log.AppendText(Environment.NewLine);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                string errorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
                log.AppendText(Environment.NewLine);
                log.AppendText(errorMessage);
                log.AppendText(Environment.NewLine);
                throw new Exception(errorMessage);
            }

            string okMessage = await httpResponseMessage.Content.ReadAsStringAsync();
            log.AppendText(Environment.NewLine);
            log.AppendText(okMessage);
            log.AppendText(Environment.NewLine);
        }
        catch (Exception ex)
        {
            log.AppendText(ex.Message);
            log.AppendText(Environment.NewLine);
            log.AppendText("********************************");
            log.AppendText(Environment.NewLine);
            throw new Exception(ex.Message);
        }

        string[] fileUrls = {
            "css/index.css",
            "lib/jquery-3.6.4.js",
            "script/map.js",
            "script/namespaces.js",
            "script/namespaces.js",
            "config.json",
            "index.html"
        };

        try
        {
            foreach (string url in fileUrls)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string milosevUrl = $"http://milosev.com/gallery/allWithPics/travelBuddies/{folderName.Text}/";
                Uri baseUri = new Uri(milosevUrl);
                Uri uri = new Uri(baseUri, url);

                HttpResponseMessage response = await HttpClientGet.GetAsync(uri.AbsoluteUri);
                if (response.IsSuccessStatusCode)
                {
                    log.AppendText(@$"File: {uri.AbsoluteUri} exists");
                }
                else
                {
                    log.AppendText(@$"Request failed with status code: {response.StatusCode}");
                }

                log.AppendText(Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            log.AppendText(ex.Message);
            log.AppendText(Environment.NewLine);
            log.AppendText("********************************");
            log.AppendText(Environment.NewLine);
            throw new Exception(ex.Message);
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        if (!cancellationTokenSourceDisposed)
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            cancellationTokenSourceDisposed = true;
        }
    }
}