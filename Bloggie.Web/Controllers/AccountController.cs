using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly IUserRepository userRepository;
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
		IUserRepository userRepository)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		[HttpGet]
		public IActionResult Register()
		{
			return View("Register");
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			if (ModelState.IsValid)
			{


				var identityUser = new IdentityUser
				{
					UserName = registerViewModel.Username,
					Email = registerViewModel.Email,

				};
				var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
				if (identityResult.Succeeded)
				{
					var roleIdentityResult = await userManager.AddToRoleAsync(identityUser, "User");
					if (roleIdentityResult.Succeeded)
					{
						//thong bao thanh cong
						return RedirectToAction("Login");
					}

				}
			}
			return View("Register");
		}
		[HttpGet]
		public IActionResult Login(string ReturnURl)
		{
			var model = new LoginViewModel
			{
				ReturnUrl = ReturnURl,
			};

			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
			if (signInResult.Succeeded)
			{
				if (!string.IsNullOrEmpty(loginViewModel.ReturnUrl))
				{
					return Redirect(loginViewModel.ReturnUrl);
				}
				return RedirectToAction("Index", "Home");
			}
			//show errors
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}


		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> EditProfile()
		{
			var user = await userManager.GetUserAsync(User);
			if (user == null) return NotFound();

			var model = new EditUserViewModel
			{
				Id = user.Id,
				Username = user.UserName,
				Email = user.Email
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile(EditUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// Kiểm tra user có tồn tại không
			var user = await userManager.GetUserAsync(User);
			if (user == null)
			{
				return RedirectToAction("Login");
			}

			// Cập nhật thông tin cơ bản
			var result = await userRepository.UpdateUserAsync(user.Id, model.Username, model.Email);
			if (!result)
			{
				ModelState.AddModelError("", "Failed to update profile");
				return View(model);
			}

			// Cập nhật mật khẩu nếu có
			if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
			{
				var passwordResult = await userRepository.ChangePasswordAsync(
					user.Id, model.CurrentPassword, model.NewPassword);
				if (!passwordResult)
				{
					ModelState.AddModelError("", "Failed to change password. Please check your current password.");
					return View(model);
				}
			}

			// Cập nhật lại session
			await signInManager.RefreshSignInAsync(user);

			TempData["SuccessMessage"] = "Profile updated successfully";
			return RedirectToAction("EditProfile");
		}

	}
}
