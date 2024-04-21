using Core.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Identity.TagHelpers
{
    [HtmlTargetElement("getUserInfo")]
    public class GetUserInfo:TagHelper
    {
        private readonly UserManager<AppUser> _userManager;
        public int UserId { get; set; }
        public GetUserInfo(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var html = "";
            var user= await _userManager.Users.SingleOrDefaultAsync(x=>x.Id==UserId);
            var roles=await _userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                html += item + " ";
            }
            output.Content.SetHtmlContent(html);
        }
    }
}
