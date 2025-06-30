namespace BadluckMusicApi.Services
{
    public class LocalFileService : IFileService
    {
        private readonly ILogger<LocalFileService> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileService(
            ILogger<LocalFileService> logger, 
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            try
            {
                await Task.Run(() => File.Delete(_env.WebRootPath + filePath));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении файла {FilePath}", filePath);
                return false;
            }
        }

        public async Task<string> GetFullFilePathAsync(string filePath)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return string.Empty;

            return $"{request.Scheme}://{request.Host}{filePath}";
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
                var directory = _env.WebRootPath;

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