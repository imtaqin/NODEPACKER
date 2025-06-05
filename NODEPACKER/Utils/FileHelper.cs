using System;
using System.IO;

namespace NodeJsPacker.Utils
{
    public static class FileHelper
    {
        public static void CopyDirectoryExcludeNodeModules(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(targetDir, fileName), true);
            }

            foreach (string directory in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(directory);
                if (dirName.Equals("node_modules", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Skipping existing node_modules directory - will install fresh dependencies");
                    continue;
                }

                CopyDirectoryExcludeNodeModules(directory, Path.Combine(targetDir, dirName));
            }
        }

        public static string FindNodeExecutable(string nodePath, string nodeExe)
        {
            string[] possiblePaths = {
                Path.Combine(nodePath, nodeExe),
                Path.Combine(nodePath, "bin", nodeExe)
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path)) return path;
            }

            var files = Directory.GetFiles(nodePath, nodeExe, SearchOption.AllDirectories);
            if (files.Length > 0) return files[0];

            return Path.Combine(nodePath, nodeExe);
        }

        public static string GetNpmScript(string nodePath)
        {
            string[] possiblePaths = {
                Path.Combine(nodePath, "lib", "node_modules", "npm", "bin", "npm-cli.js"),
                Path.Combine(nodePath, "node_modules", "npm", "bin", "npm-cli.js")
            };

            foreach (string path in possiblePaths)
            {
                if (File.Exists(path)) return path;
            }

            var files = Directory.GetFiles(nodePath, "npm-cli.js", SearchOption.AllDirectories);
            return files.Length > 0 ? files[0] : null;
        }
    }
}