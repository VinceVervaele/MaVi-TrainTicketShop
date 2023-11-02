using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using Trains_FSD.Areas.Data;
using Trains_FSD.ViewModels;

namespace Trains_FSD.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AccountController(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationVM userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
            var existingUser = await _userManager.FindByEmailAsync(userModel.Email);
            if (existingUser == null)
            {
                var user = _mapper.Map<User>(userModel);
                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user, userModel.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userModel);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "You already have an account.");
                return View();
            }
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
