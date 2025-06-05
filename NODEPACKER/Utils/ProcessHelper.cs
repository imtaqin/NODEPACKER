using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace NodeJsPacker.Utils
{
    public static class ProcessHelper
    {
        public static async Task RunCommandAsync(string command, string arguments, string workingDirectory = null)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory()
                }
            };

            process.Start();
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                Console.WriteLine($"Command output: {output}");
                Console.WriteLine($"Command error: {error}");
                throw new Exception($"Command failed with exit code {process.ExitCode}: {error}");
            }
        }
    }
}