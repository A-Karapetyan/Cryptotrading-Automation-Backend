using CA.BLL.Services;
using CA.DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptovalue_Automation.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public Task<bool> VerifyEmail([FromBody] VerifyModel model)
        {
            return userService.VerifyEmail(model);
        }
        [HttpPost]
        public Task<int> RegisterEmail([FromBody]  RegisterEmailModel model)
        {
            return userService.RegisterEmail(model);
        }
    }
}
