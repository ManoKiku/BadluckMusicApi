namespace BadluckMusicApi.Services
{
    public interface IFileService
    {
        Task<bool> UploadFileAsync(Stream stream, string destinationPath);
        Task<bool> DeleteFileAsync(string filePath);
    }
}
