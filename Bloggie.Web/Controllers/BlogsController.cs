using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bloggie.Web.Controllers
{
    [Route("[controller]")]
    public class BlogsController : Controller
    {
        private readonly ILogger<BlogsController> _logger;
        private readonly IBlogPostRepository blogPostRepository;

        public BlogsController(ILogger<BlogsController> logger,IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {   
            var blogPort=await blogPostRepository.GetByUrlHandleAsync(urlHandle);
            return View(blogPort);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}