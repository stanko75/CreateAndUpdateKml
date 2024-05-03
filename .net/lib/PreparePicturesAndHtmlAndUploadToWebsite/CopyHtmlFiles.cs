namespace PreparePicturesAndHtmlAndUploadToWebsite;

public class CopyHtmlFiles: ICopyHtmlFiles
{
    //copy HTML template for blog (html\blog) to another folder (prepareForUpload/nameofAlbum)
    public void CopyHtmlTemplateForBlog(
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