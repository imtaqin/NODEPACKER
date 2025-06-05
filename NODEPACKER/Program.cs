using NodeJsPacker.Services;
using System;
using System.Threading.Tasks;

namespace NodeJsPacker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: NodeJsPacker <source-directory> <output-executable>");
                return;
            }

            try
            {
                var packer = new NodeJsPackerService();
                await packer.PackAsync(args[0], args[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}