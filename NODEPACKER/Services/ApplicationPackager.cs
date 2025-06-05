using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NodeJsPacker.Utils;

namespace NodeJsPacker.Services
{
    public class ApplicationPackager : IApplicationPackager
    {
        public async Task<string> CreatePackageAsync(string sourceDir, string nodePath, string tempDir)
        {
            string packageDir = Path.Combine(tempDir, "package");
            Directory.CreateDirectory(packageDir);

            string appDir = Path.Combine(packageDir, "app");
            FileHelper.CopyDirectoryExcludeNodeModules(sourceDir, appDir);

            await CopyNodeExecutable(nodePath, packageDir);
            await InstallDependencies(nodePath, packageDir, appDir);

            return packageDir;
        }

        private async Task CopyNodeExecutable(string nodePath, string packageDir)
        {
            string nodeExe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "node.exe" : "node";
            string nodeExecutable = FileHelper.FindNodeExecutable(nodePath, nodeExe);

            if (!File.Exists(nodeExecutable))
                throw new FileNotFoundException($"Node executable not found at: {nodeExecutable}");

            File.Copy(nodeExecutable, Path.Combine(packageDir, nodeExe));
        }

        private async Task InstallDependencies(string nodePath, string packageDir, string appDir)
        {
            if (!File.Exists(Path.Combine(appDir, "package.json")))
                return;

            Console.WriteLine("Installing dependencies...");
            string npmScript = FileHelper.GetNpmScript(nodePath);

            if (!string.IsNullOrEmpty(npmScript))
            {
                string nodeExe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "node.exe" : "node";
                await ProcessHelper.RunCommandAsync(
                    Path.Combine(packageDir, nodeExe),
                    $"\"{npmScript}\" install --production",
                    appDir);
            }
            else
            {
                Console.WriteLine("Warning: npm not found, skipping dependency installation");
            }
        }
    }
}