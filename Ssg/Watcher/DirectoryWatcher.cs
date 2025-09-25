using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ssg.Watcher
{
    public class DirectoryWatcher : IDisposable
    {
        private readonly FileSystemWatcher _watcher;
        private readonly Action _onChange;
        private readonly Timer _debounceTimer;
        private const int DebounceMs = 300;

        public DirectoryWatcher(string path, Action onChange)
        {
            _onChange = onChange;
            _debounceTimer = new Timer(_ => _onChange(), null, Timeout.Infinite, Timeout.Infinite);

            _watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = false,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            _watcher.Changed += FileChanged;
            _watcher.Created += FileChanged;
            _watcher.Deleted += FileChanged;
            _watcher.Renamed += FileChanged;
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (!e.FullPath.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                && !e.FullPath.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
                && !e.FullPath.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
                return;

            // Debounce multiple rapid events
            _debounceTimer.Change(DebounceMs, Timeout.Infinite);
        }

        public void Start() => _watcher.EnableRaisingEvents = true;
        public void Stop() => _watcher.EnableRaisingEvents = false;
        public void Dispose()
        {
            _watcher?.Dispose();
            _debounceTimer?.Dispose();
        }
    }
}
