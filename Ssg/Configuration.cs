using System;
using System.IO;

namespace Ssg
{
    public enum TemplateEngine
    {
        Simple,
        RazorLight
    }

    public record Configuration
    {
        public string SourceDir { get; init; } = "content";
        public string OutputDir { get; init; } = "www";
        public bool Watch { get; init; } = false;
        public TemplateEngine TemplateEngine { get; init; } = TemplateEngine.Simple;
        public string LayoutFile { get; init; } = "layout.html";

        public static Configuration LoadFromArgs(string[] args)
        {
            // Reasonable defaults; do not prompt the user.
            var cfg = new Configuration();
            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                if (a == "--src" && i + 1 < args.Length) cfg = cfg with { SourceDir = args[++i] };
                if (a == "--out" && i + 1 < args.Length) cfg = cfg with { OutputDir = args[++i] };
                if (a == "--watch") cfg = cfg with { Watch = true };
                if (a == "--razor") cfg = cfg with { TemplateEngine = TemplateEngine.RazorLight };
                if (a == "--layout" && i + 1 < args.Length) cfg = cfg with { LayoutFile = args[++i] };
            }

            // Ensure directories are absolute
            cfg = cfg with
            {
                SourceDir = Path.GetFullPath(cfg.SourceDir),
                OutputDir = Path.GetFullPath(cfg.OutputDir)
            };

            return cfg;
        }
    }
}
