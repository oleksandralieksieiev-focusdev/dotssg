using Ssg.Models;
using System.Threading.Tasks;

namespace Ssg.Services
{
    public interface ITemplateService
    {
        Task<string> RenderAsync(PageModel model);
    }
}
