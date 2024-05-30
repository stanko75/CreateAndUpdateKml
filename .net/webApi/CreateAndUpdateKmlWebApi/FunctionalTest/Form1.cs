using FunctionalTest.Log;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FunctionalTest;

public partial class Form1 : Form
{
    private CancellationTokenSource? _cancellationTokenSource = new();
    private bool _cancellationTokenSourceDisposed;
    private static readonly HttpClient HttpClientPost = new();

    private const string JsonConfigForTests = "jsconfigForTests.json";

    public Form1()
    {
        InitializeComponent();
        if (File.Exists(JsonConfigForTests))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile(JsonConfigForTests)
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
        var jsonConfig = new JObject
        {
            ["address"] = address.Text,
            ["gpsLocationsPath"] = tbGpsLocationsPath.Text,
            ["ftpUser"] = tbFtpUser.Text,
            ["ftpHost"] = tbFtpHost.Text,
            ["ftpPass"] = tbFtpPass.Text,
            ["folderName"] = folderName.Text,
            ["kmlFileName"] = kmlFileName.Text,
            ["imagesPath"] = imagesPath.Text
        };

        string json = jsonConfig.ToString(Formatting.Indented);
        File.WriteAllText(@$"..\..\..\{JsonConfigForTests}", json);
    }

    private async void PostGpsPositionsFromFilesWithFileName_Click(object sender, EventArgs e)
    {
        var command = new PostGpsPositionsFromFilesWithFileNameCommand
        {
            AddressText = address.Text,
            KmlFileName = kmlFileName.Text,
            FolderName = folderName.Text,
            HttpClientPost = HttpClientPost,
            GpsLocationsPath = tbGpsLocationsPath.Text,
            CancellationToken = _cancellationTokenSource?.Token
        };

        ICommandHandler<PostGpsPositionsFromFilesWithFileNameCommand> handler =
            new PostGpsPositionsFromFilesWithFileNameHandler(new TextBoxLogger(log));

        await ExecuteHandler(command, logger => new PostGpsPositionsFromFilesWithFileNameHandler(logger));
    }

    private async void UploadImage_Click(object sender, EventArgs e)
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

        await ExecuteHandler(command, logger => new UploadImageHandler(logger));
    }

    private async void UploadToBlog_Click(object sender, EventArgs e)
    {
        var command = new UploadToBlogCommand
        {
            AddressText = address.Text,
            KmlFileName = kmlFileName.Text,
            FolderName = folderName.Text,
            HttpClientPost = HttpClientPost,
            CancellationToken = _cancellationTokenSource.Token,
        };

        await ExecuteHandler(command, logger => new UploadToBlogHandler(logger));
    }

    private async Task ExecuteHandler<TCommand>(TCommand command, Func<ILogger, ICommandHandler<TCommand>> handlerFactory)
    {
        if (_cancellationTokenSource != null)
        {
            var logger = new TextBoxLogger(log);

            try
            {
                ICommandHandler<TCommand> handler = handlerFactory(logger);
                await handler.Execute(command);
            }
            catch (OperationCanceledException)
            {
                logger.Log("Canceled.");
            }
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel();

        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationTokenSourceDisposed = false;
    }
}