using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NodeJsPacker.Templates;
using NodeJsPacker.Utils;

namespace NodeJsPacker.Services
{
    public class ExecutableGenerator : IExecutableGenerator
    {
        public async Task CreateExecutableAsync(string packagePath, string outputPath)
        {
            string launcherCode = GenerateLauncherCode();
            string projectDir = Path.Combine(packagePath, "launcher_project");
            Directory.CreateDirectory(projectDir);

            await SetupLauncherProject(packagePath, projectDir, launcherCode);
            await CompileExecutable(projectDir, outputPath);
        }

        private async Task SetupLauncherProject(string packagePath, string projectDir, string launcherCode)
        {
            string programPath = Path.Combine(projectDir, "Program.cs");
            await File.WriteAllTextAsync(programPath, launcherCode);

            string resourcePath = Path.Combine(packagePath, "app.zip");
            ZipFile.CreateFromDirectory(Path.Combine(packagePath, "app"), resourcePath);

            File.Copy(resourcePath, Path.Combine(projectDir, "app.zip"));

            string nodeExePath = Path.Combine(packagePath,
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "node.exe" : "node");
            File.Copy(nodeExePath, Path.Combine(projectDir, "node.exe"));
        }

        private async Task CompileExecutable(string projectDir, string outputPath)
        {
            string cscPath = CompilerHelper.FindCscPath();
            if (cscPath == "csc")
            {
                throw new System.Exception("C# compiler not found. Please install Visual Studio or .NET SDK.");
            }

            string programPath = Path.Combine(projectDir, "Program.cs");
            string appZipPath = Path.Combine(projectDir, "app.zip");
            string nodeExePath = Path.Combine(projectDir, "node.exe");

            string compileArgs = $"/target:exe /out:\"{outputPath}\" " +
                               $"/reference:System.IO.Compression.dll " +
                               $"/reference:System.IO.Compression.FileSystem.dll " +
                               $"/resource:\"{appZipPath}\",app.zip " +
                               $"/resource:\"{nodeExePath}\",node.exe " +
                               $"\"{programPath}\"";

            await ProcessHelper.RunCommandAsync(cscPath, compileArgs);
        }

        public string GenerateLauncherCode()
        {
            return LauncherCodeTemplate.GetLauncherCode();
        }
    }
}