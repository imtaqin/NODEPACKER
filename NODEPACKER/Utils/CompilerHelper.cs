using System.IO;
using System.Runtime.InteropServices;

namespace NodeJsPacker.Utils
{
    public static class CompilerHelper
    {
        public static string FindCscPath()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string[] possiblePaths = {
                    @"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\Roslyn\csc.exe",
                    @"C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\Roslyn\csc.exe",
                    @"C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\Roslyn\csc.exe",
                    @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\Roslyn\csc.exe",
                    @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
                };

                foreach (string path in possiblePaths)
                {
                    if (File.Exists(path)) return path;
                }
            }

            return "csc";
        }
    }
}