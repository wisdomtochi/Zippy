using Zippy.Models;

namespace Zippy.Services
{
    public interface IResourceService
    {
        Task<string> GetOriginalURL(string key);
        Task<string> GenerateShortenedURL(ResourceViewModel model);
    }
}
