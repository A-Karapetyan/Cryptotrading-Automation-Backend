using CA.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptovalue_Automation.Autorization
{
    public class UserPermission : IAuthorizationFilter
    {
        private readonly IUserService _userService;

        public UserPermission(IUserService userService)
        {
            _userService = userService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Claims.Any())
            {
                context.Result = new JsonResult("Invalid token") { StatusCode = 401 };
                return;
            }
            var user = _userService.CheckPersonById(int.Parse(context.HttpContext.User.Claims.Single(u => u.Type == "personId").Value));
            if (user == null)
            {
                context.Result = new JsonResult("Unauthorized") { StatusCode = 401 };
                return;
            }
        }
    }
}
