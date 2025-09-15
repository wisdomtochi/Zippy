using Microsoft.EntityFrameworkCore;
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

        public async Task<Result<string>> GenerateShortenedURL(ResourceViewModel model)
        {
            try
            {
                if (Uri.TryCreate(model.Url, UriKind.Absolute, out var uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    var existingResources = await _resourceRepo.ReadAllQuery()
                                                               .AsNoTracking()
                                                               .Select(r => new { r.Url, r.Alias })
                                                               .ToListAsync();

                    if (!string.IsNullOrWhiteSpace(model.Alias) && existingResources.Any(r => r.Alias == model.Alias))
                    {
                        return Result<string>.Failure("The provided alias is already in use. Please choose a different alias.");
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

                    if (!string.IsNullOrWhiteSpace(model.Alias)) return Result<string>.Success(model.Alias);

                    return Result<string>.Success(key);
                }

                return Result<string>.Failure("Oops! The link seems invalid. Try adding http:// or https:// at the start.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the shortened URL.", ex);
            }
        }

        public async Task<Result<string>> GetOriginalURL(string key)
        {
            try
            {
                var resource = await _resourceRepo.ReadAllQuery()
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(r => r.Key == key || r.Alias == key);

                if (resource == null) return Result<string>.Failure("The provided alias is already in use. Please choose a different alias.");

                if (!resource.Url.StartsWith("http") && !resource.Url.StartsWith("https") || resource.Url.StartsWith("www"))
                    return Result<string>.Success($"https://{resource.Url}");

                return Result<string>.Success(resource.Url);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the original URL.", ex);
            }
        }
    }
}
