using CA.BLL.Services;
using CA.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptovalue_Automation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<bool> VerifyEmail([FromBody] VerifyModel model)
        {
            return await userService.VerifyEmail(model);
        }

        [HttpPost]
        public async Task<int> RegisterEmail([FromBody]  RegisterEmailModel model)
        {
            return await userService.RegisterEmail(model);
        }

        [HttpPost]
        public async Task<LoginTokenModel> Register([FromBody] RegisterModel model)
        {
            return await userService.Register(model);
        }

        [HttpPost]
        public async Task<LoginTokenModel> Login([FromBody] LoginModel model)
        {
            return await userService.Login(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<string> GetUserEmail()
        {
            return await userService.GetUserEmail(GetUserIdFromToken());
        }
    }
}
