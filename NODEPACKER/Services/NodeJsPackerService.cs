using System;
using System.IO;
using System.Threading.Tasks;
using NodeJsPacker.Utils;

namespace NodeJsPacker.Services
{
    public class NodeJsPackerService
    {
        private readonly INodeJsDownloader _downloader;
        private readonly IApplicationPackager _packager;
        private readonly IExecutableGenerator _generator;

        public NodeJsPackerService()
        {
            _downloader = new NodeJsDownloader();
            _packager = new ApplicationPackager();
            _generator = new ExecutableGenerator();
        }

        public async Task PackAsync(string sourceDirectory, string outputPath)
        {
            Console.WriteLine("Starting Node.js application packaging...");

            string tempDir = Path.Combine(Path.GetTempPath(), "nodepacker_" + DateTime.Now.Ticks);
            Directory.CreateDirectory(tempDir);

            try
            {
                string nodeVersion = await _downloader.GetLatestLTSVersionAsync();
                Console.WriteLine($"Found LTS version: {nodeVersion}");

                string nodePath = await _downloader.DownloadNodeAsync(nodeVersion, tempDir);
                Console.WriteLine("Node.js downloaded successfully");

                string packagedApp = await _packager.CreatePackageAsync(sourceDirectory, nodePath, tempDir);
                Console.WriteLine("Application packaged");

                await _generator.CreateExecutableAsync(packagedApp, outputPath);
                Console.WriteLine($"Executable created: {outputPath}");
            }
            finally
            {
                await DirectoryHelper.SafeDeleteDirectoryAsync(tempDir);
            }
        }
    }
}