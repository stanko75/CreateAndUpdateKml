namespace PreparePicturesAndHtmlAndUploadToWebsite;

public interface IMirrorDirAndFileStructureOnFtp
{
    public Task? Execute(string localRootFolderWithRelativePathToCopy, string remoteRootFolder);
}