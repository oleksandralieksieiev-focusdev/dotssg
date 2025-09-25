using System.IO;

namespace Ssg.Utils
{
    public static class FileHelpers
    {
        public static void CopyStaticAssets(string sourceDir, string outputDir)
        {
            // Copy non-md files (css, js, images) to output preserving structure
            foreach (var file in Directory.EnumerateFiles(sourceDir, "*.*", SearchOption.AllDirectories))
            {
                if (file.EndsWith(".md", System.StringComparison.OrdinalIgnoreCase)) continue;
                var rel = Path.GetRelativePath(sourceDir, file);
                var dest = Path.Combine(outputDir, rel);
                var destDir = Path.GetDirectoryName(dest);
                if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                File.Copy(file, dest, true);
            }
        }
    }
}
