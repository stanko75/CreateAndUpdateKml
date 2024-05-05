namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class CopyHtmlFiles: ICopyHtmlFiles
{
    //copy HTML template for blog (html/blog) to another folder (prepareForUpload/nameofAlbum)
    public void CopyHtmlTemplateForBlog_old(
        string htmlTemplateFolderWithRelativePath
        , string rootFolderWithRelativePathToCopy
        , string nameOfAlbum
        , string kmlFileName)
    {
        if (!Directory.Exists(htmlTemplateFolderWithRelativePath))
        {
            throw new DirectoryNotFoundException($"The folder {Path.GetFullPath(htmlTemplateFolderWithRelativePath)} does not exist!");
        }

        if (!File.Exists(kmlFileName))
        {
            throw new FileNotFoundException(
                $"The file {kmlFileName} does not exist. Absolute path: {Path.GetFullPath(kmlFileName)}");
        }

        CopyFilesRecursively(htmlTemplateFolderWithRelativePath,
            Path.Join(rootFolderWithRelativePathToCopy, nameOfAlbum));

        File.Copy(kmlFileName,  Path.Join(rootFolderWithRelativePathToCopy, Path.Join(nameOfAlbum, Path.GetFileName(kmlFileName))), true);
    }

    /*
      copy existing structure:

     \nameOfAlbum\nameOfAlbumThumbs.json
     \nameOfAlbum\pics
     \nameOfAlbum\thumbs
     \nameOfAlbum\nameOfAlbum.kml

    to 

       string[] fileAndFolderStructure =
       [
           @"kml\nameOfAlbum.kml",
           "pics",
           "thumbs",
           "www",
           @"www\css\index.css",
           @"www\lib\jquery-3.3.1.js",
           @"www\script\map.js",
           @"www\script\namespaces.js",
           @"www\script\pics2maps.js",
           @"www\script\thumbnails.js",
           @"www\index.html",
           @"www\joomlaPreview.html",
           @"www\/*albumName* /.js",
           @"www\/*albumName* /Thumbs.json"
       ];

     */

    public void CopyHtmlTemplateForBlog(
        string htmlTemplateFolderWithRelativePath //"html\blog\www"
        , string prepareForUploadFolder //"prepareForUpload"
        , string nameOfAlbum
        , string kmlFileName)
    {
        if (!Directory.Exists(htmlTemplateFolderWithRelativePath))
        {
            throw new DirectoryNotFoundException($"The folder {Path.GetFullPath(htmlTemplateFolderWithRelativePath)} does not exist!");
        }

        if (!File.Exists(kmlFileName))
        {
            throw new FileNotFoundException(
                $"The file {kmlFileName} does not exist. Absolute path: {Path.GetFullPath(kmlFileName)}");
        }

        if (Directory.Exists(prepareForUploadFolder))
        {
            Directory.Delete(prepareForUploadFolder, true);
        }

        prepareForUploadFolder = Path.Join(prepareForUploadFolder, nameOfAlbum);
        string wwwFolder = Path.Join(prepareForUploadFolder, "www");
        Directory.CreateDirectory(prepareForUploadFolder);
        Directory.CreateDirectory(Path.Join(wwwFolder, "script"));
        Directory.CreateDirectory(Path.Join(wwwFolder, "css"));
        Directory.CreateDirectory(Path.Join(wwwFolder, "lib"));

        //copy from "html\blog\www" to prepareForUpload
        string[] allFiles = Directory.GetFiles(htmlTemplateFolderWithRelativePath, "*.*", SearchOption.AllDirectories);
        foreach (string file in allFiles)
        {
            string fileNameOnly = Path.GetFileName(file);
            string savewwwFiles = Path.Join(wwwFolder, fileNameOnly);
            if (file.ToLower().Contains(@"www\script"))
            {
                savewwwFiles = Path.Join(wwwFolder, Path.Join("script", fileNameOnly));
            } 
            else if (file.ToLower().Contains(@"www\css"))
            {
                savewwwFiles = Path.Join(wwwFolder, Path.Join("css", fileNameOnly));
            }
            else if (file.ToLower().Contains(@"www\lib"))
            {
                savewwwFiles = Path.Join(wwwFolder, Path.Join("lib", fileNameOnly));
            }

            File.Copy(file, savewwwFiles);
        }
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        Directory.CreateDirectory(targetPath);

        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
        }
    }
}