using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;

namespace Bloggie.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly UserManager<IdentityUser> userManager;

        public ProfileController(IUserProfileRepository userProfileRepository, UserManager<IdentityUser> userManager)
        {
            this.userProfileRepository = userProfileRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            var profile = await userProfileRepository.GetByUserIdAsync(user.Id);
            
            if (profile == null)
                return RedirectToAction("Create");

            var viewModel = new ProfileViewModel
            {
                Id = profile.Id,
                FullName = profile.FullName,
                DateOfBirth = profile.DateOfBirth,
                Address = profile.Address,
                PhoneNumber = profile.PhoneNumber,
                Education = profile.Education,
                WorkExperience = profile.WorkExperience,
                Skills = profile.Skills
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProfileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await userManager.GetUserAsync(User);
                
                var profile = new UserProfile
                {
                    UserId = user.Id,
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Education = model.Education,
                    WorkExperience = model.WorkExperience,
                    Skills = model.Skills
                };

                await userProfileRepository.AddAsync(profile);
                TempData["Success"] = "Hồ sơ đã được tạo thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await userManager.GetUserAsync(User);
            var profile = await userProfileRepository.GetByUserIdAsync(user.Id);

            if (profile == null)
                return RedirectToAction("Create");

            var viewModel = new ProfileViewModel
            {
                Id = profile.Id,
                FullName = profile.FullName,
                DateOfBirth = profile.DateOfBirth,
                Address = profile.Address,
                PhoneNumber = profile.PhoneNumber,
                Education = profile.Education,
                WorkExperience = profile.WorkExperience,
                Skills = profile.Skills
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Preview()
        {
            var user = await userManager.GetUserAsync(User);
            var profile = await userProfileRepository.GetByUserIdAsync(user.Id);
            
            if (profile == null)
                return RedirectToAction("Create");

            var viewModel = new ProfileViewModel
            {
                Id = profile.Id,
                FullName = profile.FullName,
                DateOfBirth = profile.DateOfBirth,
                Address = profile.Address,
                PhoneNumber = profile.PhoneNumber,
                Education = profile.Education,
                WorkExperience = profile.WorkExperience,
                Skills = profile.Skills
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var user = await userManager.GetUserAsync(User);
                var profile = await userProfileRepository.GetByUserIdAsync(user.Id);

                if (profile == null)
                    return RedirectToAction("Create");

                profile.FullName = model.FullName;
                profile.DateOfBirth = model.DateOfBirth;
                profile.Address = model.Address;
                profile.PhoneNumber = model.PhoneNumber;
                profile.Education = model.Education;
                profile.WorkExperience = model.WorkExperience;
                profile.Skills = model.Skills;

                await userProfileRepository.UpdateAsync(profile);
                TempData["Success"] = "Hồ sơ đã được cập nhật thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(model);
            }
        }
    }
}