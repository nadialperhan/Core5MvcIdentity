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
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()           
        {
            var list = _roleManager.Roles.ToList();
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
           
            return View(new RoleCreateModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var userrole = new AppRole()
                {
                    Name = model.Name,
                    CreatedTime=DateTime.Now
                };
                var result = await _roleManager.CreateAsync(userrole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }
    }
}
