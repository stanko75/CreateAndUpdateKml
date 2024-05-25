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
    CancellationTokenSource? _cancellationTokenSource = new();
    private bool _cancellationTokenSourceDisposed;
    private static readonly HttpClient HttpClientPost = new();
    private static readonly HttpClient HttpClientGet = new();

    private const string JsconfigForTests = "jsconfigForTests.json";

    public Form1()
    {
        InitializeComponent();
        if (File.Exists(JsconfigForTests))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(JsconfigForTests)
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
        File.WriteAllText(@$"..\..\..\{JsconfigForTests}", json);
    }

    private void PostGpsPositionsFromFilesWithFileName_Click(object sender, EventArgs e)
    {
        if (_cancellationTokenSourceDisposed)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        if (_cancellationTokenSource != null)
        {
            var command = new PostGpsPositionsFromFilesWithFileNameCommand
            {
                AddressText = address.Text,
                KmlFileName = kmlFileName.Text,
                FolderName = folderName.Text,
                HttpClientPost = HttpClientPost,
                GpsLocationsPath = tbGpsLocationsPath.Text,
                CancellationToken = _cancellationTokenSource.Token
            };

            ICommandHandler<PostGpsPositionsFromFilesWithFileNameCommand> handler =
                new PostGpsPositionsFromFilesWithFileNameHandler(new TextBoxLogger(log));

            Task task = handler.Execute(command);
            log.AppendText(task.Status.ToString());
        }

        log.AppendText(Environment.NewLine);
    }

    private void UploadImage_Click(object sender, EventArgs e)
    {
        if (_cancellationTokenSourceDisposed)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        if (_cancellationTokenSource != null)
        {
            var command = new UploadImageCommand
            {
                AddressText = address.Text,
                KmlFileName = kmlFileName.Text,
                FolderName = folderName.Text,
                HttpClientPost = HttpClientPost,
                CancellationToken = _cancellationTokenSource.Token,
                ImagesPath = imagesPath.Text
            };

            ICommandHandler<UploadImageCommand> handler =
                new UploadImageHandler(new TextBoxLogger(log));

            Task task = handler.Execute(command);
            log.AppendText(task.Status.ToString());
        }
    }

    private void UploadToBlog_Click(object sender, EventArgs e)
    {
        if (_cancellationTokenSourceDisposed)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        if (_cancellationTokenSource != null)
        {
            var command = new UploadToBlogCommand
            {
                AddressText = address.Text,
                KmlFileName = kmlFileName.Text,
                FolderName = folderName.Text,
                HttpClientPost = HttpClientPost,
                CancellationToken = _cancellationTokenSource.Token,
            };

            ICommandHandler<UploadToBlogCommand> handler =
                new UploadToBlogHandler(new TextBoxLogger(log));

            Task task = handler.Execute(command);
            log.AppendText(task.Status.ToString());
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        if (!_cancellationTokenSourceDisposed)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _cancellationTokenSourceDisposed = true;
        }
    }
}