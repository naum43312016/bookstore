using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = SD.ManagerUser)]
        public IActionResult Index()
        {
            List<ApplicationUser> applicationUsers = _db.ApplicationUser.ToList();
            return View(applicationUsers);
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(string email,string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(email,
                           password, true, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = "Email Or Password incorrect";
                    TempData["sign"] = "signin";
                    return RedirectToAction("Index", "Home");
                }
            }
            var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
            return Content(allErrors.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string email,string password)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Email = email, UserName= password };
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.CustomerEndUser);//add role to user
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = "Email Or Password incorrect";
                    TempData["sign"] = "signup";
                    return RedirectToAction("Index", "Home");
                    /*foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        return Content(error.Description);
                    }*/
                }
            }
            TempData["error"] = "Email Or Password incorrect";
            TempData["sign"] = "signup";
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}