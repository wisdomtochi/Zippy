using Zippy.Models;

namespace Zippy.Services
{
    public interface IResourceService
    {
        Task<string> GetOriginalURL(string encodedUrl);
        Task<string> GenerateShortenedURL(ResourceViewModel model);
    }
}
