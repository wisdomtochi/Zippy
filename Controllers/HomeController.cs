using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Zippy.Models;
using Zippy.Services;

namespace Zippy.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IResourceService resourceService, IConfiguration configuration) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IResourceService _resourceService = resourceService;
        private readonly IConfiguration configuration = configuration;

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] string? name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await _resourceService.GetOriginalURL(name);

                if (!result.IsSuccess)
                {
                    var error = new ErrorViewModel { ErrorMessage = result.Message };

                    return View("Error", error);
                }

                return Redirect(result.Data);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ResourceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _resourceService.GenerateShortenedURL(model);

                if (!result.IsSuccess)
                {
                    var error = new ErrorViewModel { ErrorMessage = result.Message };

                    return View("Error", error);
                }

                var url = $"{configuration["BaseUrl"]}{result.Data}" ?? string.Empty;

                var successModel = new SuccessViewModel { ShortenedUrl = url };

                return View("Success", successModel);
            }

            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
