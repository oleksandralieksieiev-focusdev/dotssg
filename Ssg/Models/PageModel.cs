using System;

namespace Ssg.Models
{
    public class PageModel
    {
        public string Title { get; set; } = "Untitled";
        public string ContentHtml { get; set; } = "";
        public string SourcePath { get; set; } = "";
        public DateTime LastModifiedUtc { get; set; }
    }
}
