using Dropbox.Api;
using Dropbox.Api.Files;

namespace BadluckMusicApi.Services
{
    public class DrobBoxFileService : IFileService
    {
        private readonly string _accessToken;

        public DrobBoxFileService(string accessToken)
        {
            _accessToken = accessToken;
        }

        public async Task<bool> DeleteFileAsync(string dropboxPath)
        {
            try
            {
                using (var dbx = new DropboxClient(_accessToken))
                {
                    if (!dropboxPath.StartsWith("/"))
                        dropboxPath = "/" + dropboxPath;

                    var response = await dbx.Files.DeleteV2Async(dropboxPath);
                    return response.Metadata.IsDeleted;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deleting error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(Stream fileStream, string filePath)
        {
            try
            {
                using (var dbx = new DropboxClient(_accessToken))
                {
                    var response = await dbx.Files.UploadAsync(
                        filePath,
                        WriteMode.Overwrite.Instance,
                        body: fileStream);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload error: {ex.Message}");
                return false;
            }

        }
        public async Task<string> GetFullFilePathAsync(string dropboxPath)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                var link = await dbx.Files.GetTemporaryLinkAsync(dropboxPath);
                return link.Link;
            }
        }
    }
}
