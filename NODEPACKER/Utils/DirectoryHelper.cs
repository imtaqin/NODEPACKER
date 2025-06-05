using System;
using System.IO;
using System.Threading.Tasks;

namespace NodeJsPacker.Utils
{
    public static class DirectoryHelper
    {
        public static async Task SafeDeleteDirectoryAsync(string directory)
        {
            if (!Directory.Exists(directory)) return;

            try
            {
                SetDirectoryPermissions(directory);
                Directory.Delete(directory, true);
            }
            catch
            {
                await Task.Delay(1000);
                try
                {
                    SetDirectoryPermissions(directory);
                    Directory.Delete(directory, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: Could not clean temp directory: {ex.Message}");
                }
            }
        }

        private static void SetDirectoryPermissions(string directory)
        {
            try
            {
                var dirInfo = new DirectoryInfo(directory);
                dirInfo.Attributes = FileAttributes.Normal;

                foreach (var file in dirInfo.GetFiles("*", SearchOption.AllDirectories))
                {
                    file.Attributes = FileAttributes.Normal;
                }

                foreach (var dir in dirInfo.GetDirectories("*", SearchOption.AllDirectories))
                {
                    dir.Attributes = FileAttributes.Normal;
                }
            }
            catch { }
        }
    }
}