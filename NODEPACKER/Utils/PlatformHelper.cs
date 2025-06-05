using System;
using System.Runtime.InteropServices;

namespace NodeJsPacker.Utils
{
    public static class PlatformHelper
    {
        public static string GetNodePlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return RuntimeInformation.OSArchitecture == Architecture.X64 ? "win-x64" : "win-x86";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return RuntimeInformation.OSArchitecture == Architecture.X64 ? "linux-x64" : "linux-x86";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return RuntimeInformation.OSArchitecture == Architecture.X64 ? "darwin-x64" : "darwin-arm64";

            throw new PlatformNotSupportedException("Unsupported platform");
        }
    }
}