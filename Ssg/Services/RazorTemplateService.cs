// Optional: requires RazorLight package. Comment out if not using RazorLight.
using RazorLight;
using Ssg.Models;
using System.IO;
using System.Threading.Tasks;

namespace Ssg.Services
{
    public class RazorTemplateService : ITemplateService
    {
        private readonly RazorLightEngine _engine;
        private readonly string _layoutTemplate;

        public RazorTemplateService(string layoutPath)
        {
            _engine = new RazorLightEngineBuilder()
                .UseMemoryCachingProvider()
                .Build();

            _layoutTemplate = File.Exists(layoutPath) ? File.ReadAllText(layoutPath) :
@"@model Ssg.Models.PageModel
<!doctype html>
<html>
<head><meta charset=""utf-8""><title>@Model.Title</title></head>
<body>
<main>@Html.Raw(Model.ContentHtml)</main>
<footer><small>Generated: @Model.LastModifiedUtc.ToString(""u"")</small></footer>
</body>
</html>";
        }

        public Task<string> RenderAsync(PageModel model)
        {
            return _engine.CompileRenderStringAsync("layout", _layoutTemplate, model);
        }
    }
}
