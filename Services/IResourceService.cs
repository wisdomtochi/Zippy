using Zippy.Models;

namespace Zippy.Services
{
    public interface IResourceService
    {
        Task<Result<string>> GetOriginalURL(string key);
        Task<Result<string>> GenerateShortenedURL(ResourceViewModel model);
    }
}
