using Ssg.Models;
using System.IO;
using System.Threading.Tasks;

namespace Ssg.Services
{
    // Simple token replacement: {{Title}} and {{Content}}
    public class SimpleTemplateService : ITemplateService
    {
        private readonly string _layoutHtml;

        public SimpleTemplateService(string layoutPath)
        {
            _layoutHtml = File.Exists(layoutPath) ? File.ReadAllText(layoutPath) : DefaultLayout();
        }

        public Task<string> RenderAsync(PageModel model)
        {
            var html = _layoutHtml
                .Replace("{{Title}}", model.Title)
                .Replace("{{Content}}", model.ContentHtml)
                .Replace("{{SourcePath}}", model.SourcePath)
                .Replace("{{LastModifiedUtc}}", model.LastModifiedUtc.ToString("u"));

            return Task.FromResult(html);
        }

        private static string DefaultLayout() =>
@"<!doctype html>
<html>
<head><meta charset=""utf-8""><title>{{Title}}</title></head>
<body>
<main>{{Content}}</main>
<footer><small>Generated: {{LastModifiedUtc}}</small></footer>
</body>
</html>";
    }
}
