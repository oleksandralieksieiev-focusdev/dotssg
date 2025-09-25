using Ssg.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ssg.Services
{
    public class BuildService
    {
        private readonly Configuration _config;
        private readonly MarkdownService _md;
        private readonly ITemplateService _template;

        public BuildService(Configuration config)
        {
            _config = config;
            _md = new MarkdownService();

            // Choose template engine
            _template = config.TemplateEngine switch
            {
                TemplateEngine.RazorLight => new RazorTemplateService(Path.Combine(_config.SourceDir, _config.LayoutFile)),
                _ => new SimpleTemplateService(Path.Combine(_config.SourceDir, _config.LayoutFile))
            };
        }

        public async Task BuildAllAsync()
        {
            EnsureOutputEmpty();

            var mdFiles = Directory.EnumerateFiles(_config.SourceDir, "*.md", SearchOption.AllDirectories)
                .Where(p => !IsInOutput(p));

            var tasks = new List<Task>();
            foreach (var path in mdFiles)
            {
                tasks.Add(BuildFileAsync(path));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"Built {mdFiles.Count()} pages.");
        }

        private void EnsureOutputEmpty()
        {
            if (!Directory.Exists(_config.OutputDir))
                Directory.CreateDirectory(_config.OutputDir);
        }

        private bool IsInOutput(string path)
        {
            // Avoid building files inside output directory (if nested)
            return Path.GetFullPath(path).StartsWith(Path.GetFullPath(_config.OutputDir));
        }

        public async Task BuildFileAsync(string markdownPath)
        {
            try
            {
                var rel = Path.GetRelativePath(_config.SourceDir, markdownPath);
                var outPath = Path.Combine(_config.OutputDir, Path.ChangeExtension(rel, ".html"));
                var outDir = Path.GetDirectoryName(outPath);
                if (!Directory.Exists(outDir)) Directory.CreateDirectory(outDir);

                var mdText = _md.LoadFile(markdownPath);
                var html = _md.ConvertToHtml(mdText);

                var model = new PageModel
                {
                    Title = ExtractTitle(mdText) ?? Path.GetFileNameWithoutExtension(markdownPath),
                    ContentHtml = html,
                    SourcePath = markdownPath,
                    LastModifiedUtc = File.GetLastWriteTimeUtc(markdownPath)
                };

                var final = await _template.RenderAsync(model);
                await File.WriteAllTextAsync(outPath, final);
                Console.WriteLine($"Wrote: {outPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error building {markdownPath}: {ex.Message}");
            }
        }

        private string? ExtractTitle(string md)
        {
            using var reader = new StringReader(md);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("# "))
                    return line.Substring(2).Trim();
            }
            return null;
        }
    }
}
