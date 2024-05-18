﻿using System.Drawing;
using System.Reflection.Metadata;

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
        Directory.CreateDirectory(Path.Join(prepareForUploadFolder, "kml"));
        Directory.CreateDirectory(Path.Join(prepareForUploadFolder, "pics"));
        Directory.CreateDirectory(Path.Join(prepareForUploadFolder, "thumbs"));

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

        string destFileName = Path.Join(wwwFolder, $"{nameOfAlbum}Thumbs.json");
        if (File.Exists(destFileName))
        {
            File.Copy(Path.Join(nameOfAlbum, $"{nameOfAlbum}Thumbs.json"), destFileName);
        }

        destFileName = Path.Join(wwwFolder, $"{nameOfAlbum}.json");
        if (File.Exists(destFileName))
        {
            File.Copy(Path.Join(nameOfAlbum, $"{nameOfAlbum}.json"), destFileName);
        }

        string kmlFileNameSaveTo = Path.GetFileName(kmlFileName);
        string kmlFolder = Path.Join(prepareForUploadFolder, "kml");
        kmlFileNameSaveTo = Path.Join(kmlFolder, kmlFileNameSaveTo);
        if (File.Exists(kmlFileNameSaveTo))
        {
            File.Copy(kmlFileName, kmlFileNameSaveTo);
        }

        string pics = Path.Join(nameOfAlbum, "pics");
        if (Directory.Exists(pics))
        {
            string[] picsFiles = Directory.GetFiles(pics);
            string picsDestination = Path.Join(prepareForUploadFolder, "pics");
            foreach (string picsFile in picsFiles)
            {
                File.Copy(picsFile, Path.Join(picsDestination, Path.GetFileName(picsFile)));
            }
        }

        string thumbs = Path.Join(nameOfAlbum, "thumbs");
        if (Directory.Exists(thumbs))
        {
            string[] thumbsFiles = Directory.GetFiles(thumbs);
            string thumbsDestination = Path.Join(prepareForUploadFolder, "thumbs");
            foreach (string thumbsFile in thumbsFiles)
            {
                File.Copy(thumbsFile, Path.Join(thumbsDestination, Path.GetFileName(thumbsFile)));
            }
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