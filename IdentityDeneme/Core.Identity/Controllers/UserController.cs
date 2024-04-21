using Core.Identity.Context;
using Core.Identity.Entities;
using Core.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly NadiContext _context;

        public UserController(UserManager<AppUser> userManager, NadiContext context, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task< IActionResult> Index()
        {
            //var query = _userManager.Users;
            //var users= _context.Users.Join(_context.UserRoles, user => user.Id, userrole => userrole.RoleId,(user, userrole)=>new { 
            //    user,userrole
            //}).Join(_context.Roles,two=>two.userrole.RoleId,role=>role.Id,(two,role)=>new { two.user,two.userrole,role}) .Where(x=>x.role.Name=="Member").Select(x=>new AppUser { 
            //    Id=x.user.Id,
            //    AccessFailedCount=x.user.AccessFailedCount,
            //    ConcurrencyStamp=x.user.ConcurrencyStamp,
            //    Email=x.user.Email,
            //    EmailConfirmed=x.user.EmailConfirmed,
            //    Gender=x.user.Gender,
            //    ImagePath=x.user.ImagePath,
            //    LockoutEnabled=x.user.LockoutEnabled,
            //    LockoutEnd=x.user.LockoutEnd,

            //}).ToList();

            //var usersrole =await _userManager.GetUsersInRoleAsync("Member");
            //return View(usersrole);
            List<AppUser> filtredUsers = new List<AppUser>();
            var users = _userManager.Users.ToList();
            foreach (var item in users)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (!roles.Contains("Admin"))
                {
                    filtredUsers.Add(item);
                }

            }
            return View(filtredUsers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserAdminCreateModel());
        }
        public async Task<IActionResult> Create(UserAdminCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    Gender = model.Gender,
                    Email = model.Email
                };
                var result= await _userManager.CreateAsync(user,model.UserName+"123");
                if (result.Succeeded)
                {
                    var memberRole = await _roleManager.FindByNameAsync("Member");
                    if (memberRole == null)
                    {
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Member",
                            CreatedTime = DateTime.Now
                        });
                    }
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View();
        }
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id==id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            RoleAssignSendModel model = new RoleAssignSendModel();

            List<RoleAssignListModel> list = new List<RoleAssignListModel>();
            foreach (var item in roles)
            {
                list.Add(new RoleAssignListModel()
                {
                    Name = item.Name,
                    RoleId = item.Id,
                    Exist = userRoles.Contains(item.Name)
                });
            }
            model.Roles = list;
            model.UserId = id;

            return View(model);


        }
        public async Task<IActionResult> AssignRole(RoleAssignSendModel model)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.UserId);
            var userroles =await _userManager.GetRolesAsync(user);

            foreach (var item in model.Roles)
            {
                if (item.Exist)
                {
                    if (!userroles.Contains(item.Name))
                    {
                       await _userManager.AddToRoleAsync(user, item.Name);
                    }
                }
                else
                {
                    if (userroles.Contains(item.Name))
                    {
                        await _userManager.RemoveFromRoleAsync(user, item.Name);
                    }
                }
            }
            return RedirectToAction("Index");
        }

    }
}
