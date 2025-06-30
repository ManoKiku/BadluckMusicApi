namespace BadluckMusicApi.Services
{
    public interface IFileService
    {
        Task<bool> UploadFileAsync(Stream fileStream, string filePath);
        Task<bool> DeleteFileAsync(string filePath);
        Task<string> GetFullFilePathAsync(string filePath);    
    }
}
