using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ResourceViewModel model)
        {
            if (ModelState.IsValid)
            {
                var shortenedUrl = await _resourceService.GenerateShortenedURL(model);


                if(shortenedUrl == null)
                {
                    var error = new ErrorViewModel { ErrorMessage = "The provided alias is already in use. Please choose a different alias." };

                    return View("Error", error);
                }

                var url = $"{configuration["BaseUrl"]}{shortenedUrl}" ?? string.Empty;

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
