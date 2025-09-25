using Markdig;
using System;
using System.IO;

namespace Ssg.Services
{
    public class MarkdownService
    {
        private readonly MarkdownPipeline _pipeline;

        public MarkdownService()
        {
            _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UsePipeTables()
                .UseYamlFrontMatter() // Not parsed here, but available if extended
                .Build();
        }

        public string ConvertToHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown, _pipeline);
        }

        public string LoadFile(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
