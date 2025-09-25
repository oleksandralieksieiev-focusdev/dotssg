using System;
using System.Threading.Tasks;
using Ssg.Services;
using Ssg.Watcher;

namespace Ssg
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var config = Configuration.LoadFromArgs(args);

            Console.WriteLine($"Source: {config.SourceDir}  Output: {config.OutputDir}  Watch: {config.Watch}  TemplateEngine: {config.TemplateEngine}");

            var builder = new BuildService(config);
            await builder.BuildAllAsync();

            if (config.Watch)
            {
                using var watcher = new DirectoryWatcher(config.SourceDir, async () =>
                {
                    Console.WriteLine("Change detected — rebuilding...");
                    await builder.BuildAllAsync();
                });

                watcher.Start();
                Console.WriteLine("Watching. Press Ctrl+C to exit.");
                await Task.Delay(-1);
            }

            return 0;
        }
    }
}
