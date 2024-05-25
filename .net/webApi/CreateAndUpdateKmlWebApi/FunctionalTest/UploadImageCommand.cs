namespace FunctionalTest;

public class UploadImageCommand
{
    public string KmlFileName { get; set; }
    public string FolderName { get; set; }
    public string AddressText { get; set; }
    public string ImagesPath { get; set; }
    public CancellationToken CancellationToken { get; set; }
    public HttpClient HttpClientPost { get; set; }
}