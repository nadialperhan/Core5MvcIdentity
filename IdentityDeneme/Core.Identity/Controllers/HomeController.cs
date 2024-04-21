using Core.Identity.Entities;
using Core.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Identity.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Gender = model.Gender,
                };
               
                var identityResult = await _userManager.CreateAsync(appUser, model.Password);
                if (identityResult.Succeeded)
                {
                    var memberRole = await _roleManager.FindByNameAsync("Member");
                    if (memberRole==null)
                    {
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Member",
                            CreatedTime = DateTime.Now
                        });
                    }                
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Index");
                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        public IActionResult SignIn(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View(new UserSignInModel() { ReturnUrl=returnurl});
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var signinresult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                if (signinresult.Succeeded)
                {
                    if (!String.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                   
                    var roles =await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else
                    {
                        return RedirectToAction("Panel");
                    }
                }
                else if (signinresult.IsLockedOut)
                {
                    var lockOutEnd= await _userManager.GetLockoutEndDateAsync(user);
                    ModelState.AddModelError("", $"Hesabınız {(lockOutEnd.Value.UtcDateTime-DateTime.UtcNow).Minutes} dk süreyle askıya alınmıştır.");
                }
                else
                {
                    string message = "";
                    if (user != null)
                    {
                        var failedcount = await _userManager.GetAccessFailedCountAsync(user);
                        message = $"{ (_userManager.Options.Lockout.MaxFailedAccessAttempts - failedcount)} hakkınız kalmıştır.";
                    }
                    else
                    {
                        message = "Kullanıcı adı veya şifre hatalı.";
                    }
                    ModelState.AddModelError("", message);
                }
                
            }
            return View(model);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize(Roles ="Member")]
        public IActionResult Panel()
        {
            return View();
        }
        [Authorize(Roles = "Member")]
        public IActionResult MemberPage()
        {
            return View();
        }
        public async Task< IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
