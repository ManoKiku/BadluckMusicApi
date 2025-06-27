namespace BadluckMusicApi.Services
{
    public class LocalFileService : IFileService
    {
        private readonly ILogger<LocalFileService> _logger;

        public LocalFileService(ILogger<LocalFileService> logger)
        {
            _logger = logger;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            try
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении файла {FilePath}", filePath);
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(Stream stream, string destinationPath)
        {
            if (stream == null || stream.Length == 0 || string.IsNullOrEmpty(destinationPath))
            {
                _logger.LogWarning("Некорректные параметры для загрузки файла");
                return false;
            }

            try
            {
                var directory = Path.GetDirectoryName(destinationPath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                using (var fileStream = new FileStream(
                    destinationPath,
                    FileMode.Create,
                    FileAccess.Write))
                {
                    await stream.CopyToAsync(fileStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении файла {DestinationPath}", destinationPath);
                return false;
            }
        }
    }
}