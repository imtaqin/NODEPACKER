using System.Threading.Tasks;

namespace NodeJsPacker.Services
{
    public interface INodeJsDownloader
    {
        Task<string> GetLatestLTSVersionAsync();
        Task<string> DownloadNodeAsync(string version, string tempDir);
    }
}