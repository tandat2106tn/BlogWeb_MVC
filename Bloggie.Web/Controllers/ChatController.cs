using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "User")]

    public class ChatController : Controller
    {
        public IActionResult Index()
        {

            var user = User.Identity.Name;
            ViewBag.CurrentUser = user;
            return View();



            return View();
        }
    }
}
