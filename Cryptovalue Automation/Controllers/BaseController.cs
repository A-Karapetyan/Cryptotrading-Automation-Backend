using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptovalue_Automation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected int GetUserIdFromToken()
        {
            int.TryParse(Request.HttpContext.User.FindFirst("personId").Value, out var id);

            return id;
        }
    }
}
