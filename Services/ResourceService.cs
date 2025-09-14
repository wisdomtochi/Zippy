using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Zippy.Data.Context;
using Zippy.Data.Repository.Interface;
using Zippy.Entities;
using Zippy.Models;

namespace Zippy.Services
{
    public class ResourceService(IGenericRepository<Resource> resourceGenericRepository, IConfiguration config) : IResourceService
    {
        private readonly IGenericRepository<Resource> _resourceRepo = resourceGenericRepository;
        private readonly IConfiguration config = config;

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var random = new Random();
            return new string([.. Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)])]);
        }

        public async Task<string> GenerateShortenedURL(ResourceViewModel model)
        {
            try
            {
                var existingResources = await _resourceRepo.ReadAllQuery()
                                                           .AsNoTracking()
                                                           .Select(r => new { r.Url, r.Alias })
                                                           .ToListAsync();

                if(!string.IsNullOrWhiteSpace(model.Alias) && existingResources.Any(r => r.Alias == model.Alias))
                {
                    return null;
                }

                var key = string.Empty;

                do
                {
                    key = GenerateRandomString(8);
                } while (existingResources.Select(r => r.Url).Contains(key));

                var resource = new Resource
                {
                    Id = Guid.NewGuid(),
                    Url = model.Url,
                    Key = key,
                    Alias = model.Alias,
                    CreatedAt = DateTime.UtcNow
                };

                await _resourceRepo.AddAsync(resource);
                await _resourceRepo.SaveAsync();
                return key;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while generating the shortened URL.", ex);
            }
        }

        public async Task<string> GetOriginalURL(string encodedUrl)
        {
            try
            {
                var baseUrl = config["AppSettings:BaseUrl"];
                var url = new Uri(encodedUrl);

                var domain = url.Host;

                if(domain.Equals(baseUrl, StringComparison.OrdinalIgnoreCase) == false)
                {
                    return "The provided URL does not exist.";
                }

                var key = url.AbsolutePath.TrimStart('/');

                var resource = await _resourceRepo.ReadAllQuery()
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(r => r.Key == key || r.Alias == key);

                return resource?.Url;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the original URL.", ex);
            }
        }
    }
}
