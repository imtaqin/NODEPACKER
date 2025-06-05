using System.Threading.Tasks;

namespace NodeJsPacker.Services
{
    public interface IApplicationPackager
    {
        Task<string> CreatePackageAsync(string sourceDir, string nodePath, string tempDir);
    }
}