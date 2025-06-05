namespace NodeJsPacker.Templates
{
    public static class LauncherCodeTemplate
    {
        public static string GetLauncherCode()
        {
            return @"using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;

class Program
{
    static void Main(string[] args)
    {
        string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string workingDir = Path.Combine(currentDir, ""nodeapp_runtime"");
        
        if (Directory.Exists(workingDir))
            Directory.Delete(workingDir, true);
        
        Directory.CreateDirectory(workingDir);
        
        try
        {
            ExtractEmbeddedResources(workingDir);
            
            string nodeExe = Path.Combine(workingDir, ""node"" + (Environment.OSVersion.Platform == PlatformID.Win32NT ? "".exe"" : """"));
            string appDir = Path.Combine(workingDir, ""app"");
            string mainFile = args.Length > 0 ? GetSpecifiedMainFile(appDir, args[0]) : GetMainFile(appDir);
            
            string[] nodeArgs = GetNodeArguments(args);
            
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = nodeExe,
                    Arguments = $""\""""{mainFile}\"" "" + string.Join("" "", nodeArgs),
                    UseShellExecute = false,
                    WorkingDirectory = appDir
                }
            };
            
            process.Start();
            process.WaitForExit();
            Environment.Exit(process.ExitCode);
        }
        finally
        {
            SafeCleanup(workingDir);
        }
    }
    
    static string GetSpecifiedMainFile(string appDir, string entryPoint)
    {
        string mainFile = Path.Combine(appDir, entryPoint);
        if (!File.Exists(mainFile))
        {
            Console.WriteLine($""Error: Entry point '{entryPoint}' not found in app directory"");
            Environment.Exit(1);
        }
        return mainFile;
    }
    
    static string[] GetNodeArguments(string[] args)
    {
        if (args.Length <= 1) return new string[0];
        
        string[] nodeArgs = new string[args.Length - 1];
        Array.Copy(args, 1, nodeArgs, 0, args.Length - 1);
        return nodeArgs;
    }
    
    static string GetMainFile(string appDir)
    {
        string packageJsonPath = Path.Combine(appDir, ""package.json"");
        if (File.Exists(packageJsonPath))
        {
            try
            {
                string json = File.ReadAllText(packageJsonPath);
                string mainFile = ExtractMainFromPackageJson(json);
                if (!string.IsNullOrEmpty(mainFile))
                {
                    return Path.Combine(appDir, mainFile);
                }
            }
            catch { }
        }
        
        string[] possibleMains = { ""index.js"", ""app.js"", ""main.js"", ""server.js"" };
        foreach (string main in possibleMains)
        {
            string path = Path.Combine(appDir, main);
            if (File.Exists(path)) return path;
        }
        
        return Path.Combine(appDir, ""index.js"");
    }
    
    static string ExtractMainFromPackageJson(string json)
    {
        int mainIndex = json.IndexOf(""\\""main\\"" "");
        if (mainIndex >= 0)
        {
            int colonIndex = json.IndexOf(':', mainIndex);
            int startQuote = json.IndexOf('\\""', colonIndex);
            int endQuote = json.IndexOf('\\""', startQuote + 1);
            if (startQuote >= 0 && endQuote >= 0)
            {
                return json.Substring(startQuote + 1, endQuote - startQuote - 1);
            }
        }
        return null;
    }
    
    static void ExtractEmbeddedResources(string targetDir)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        using (var stream = assembly.GetManifestResourceStream(""app.zip""))
        using (var archive = new ZipArchive(stream))
        {
            archive.ExtractToDirectory(Path.Combine(targetDir, ""app""));
        }
        
        using (var stream = assembly.GetManifestResourceStream(""node.exe""))
        using (var fileStream = File.Create(Path.Combine(targetDir, ""node"" + (Environment.OSVersion.Platform == PlatformID.Win32NT ? "".exe"" : """"))))
        {
            stream.CopyTo(fileStream);
        }
    }
    
    static void SafeCleanup(string directory)
    {
        try
        {
            if (Directory.Exists(directory))
            {
                var dirInfo = new DirectoryInfo(directory);
                dirInfo.Attributes = FileAttributes.Normal;
                
                foreach (var file in dirInfo.GetFiles(""*"", SearchOption.AllDirectories))
                    file.Attributes = FileAttributes.Normal;
                
                Directory.Delete(directory, true);
            }
        }
        catch { }
    }
}";
        }
    }
}