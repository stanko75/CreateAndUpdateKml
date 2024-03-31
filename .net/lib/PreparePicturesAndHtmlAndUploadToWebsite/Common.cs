namespace PreparePicturesAndHtmlAndUploadToWebsite;

static class Common
{
    public static Uri ConvertRelativeWindowsPathToUri(string strAbsolutePath, string relativeWindowsPath)
    {
        Uri uriAbsolute = new Uri(strAbsolutePath);
        return new Uri(uriAbsolute, relativeWindowsPath);
    }
}