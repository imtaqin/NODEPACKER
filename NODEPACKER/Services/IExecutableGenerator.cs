using System.Threading.Tasks;

namespace NodeJsPacker.Services
{
    public interface IExecutableGenerator
    {
        Task CreateExecutableAsync(string packagePath, string outputPath);
        string GenerateLauncherCode();
    }
}