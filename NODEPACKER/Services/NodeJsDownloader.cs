using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using NodeJsPacker.Models;
using NodeJsPacker.Utils;

namespace NodeJsPacker.Services
{
    public class NodeJsDownloader : INodeJsDownloader, IDisposable
    {
        private readonly HttpClient _httpClient;
        private const string NODE_DIST_URL = "https://nodejs.org/dist/";

        public NodeJsDownloader()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetLatestLTSVersionAsync()
        {
            string response = await _httpClient.GetStringAsync($"{NODE_DIST_URL}index.json");
            var releases = JsonSerializer.Deserialize<NodeJsRelease[]>(response);

            foreach (var release in releases)
            {
                if (release.Lts?.ToString() != "False" && release.Lts != null)
                {
                    return release.Version;
                }
            }

            throw new Exception("No LTS version found");
        }

        public async Task<string> DownloadNodeAsync(string version, string tempDir)
        {
            string platform = PlatformHelper.GetNodePlatform();
            string nodeFileName = $"node-{version}-{platform}";
            string archiveExt = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".zip" : ".tar.gz";
            string downloadUrl = $"{NODE_DIST_URL}{version}/{nodeFileName}{archiveExt}";

            string archivePath = Path.Combine(tempDir, $"{nodeFileName}{archiveExt}");

            Console.WriteLine($"Downloading Node.js from: {downloadUrl}");
            using (var response = await _httpClient.GetAsync(downloadUrl))
            {
                response.EnsureSuccessStatusCode();
                using (var fileStream = File.Create(archivePath))
                {
                    await response.Content.CopyToAsync(fileStream);
                }
            }

            return await ExtractNodeArchive(archivePath, tempDir);
        }

        private async Task<string> ExtractNodeArchive(string archivePath, string tempDir)
        {
            string extractPath = Path.Combine(tempDir, "node");
            Directory.CreateDirectory(extractPath);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (var archive = ZipFile.OpenRead(archivePath))
                {
                    archive.ExtractToDirectory(extractPath);
                }
            }
            else
            {
                await ProcessHelper.RunCommandAsync("tar", $"-xzf \"{archivePath}\" -C \"{extractPath}\" --strip-components=1");
            }

            File.Delete(archivePath);
            return extractPath;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}