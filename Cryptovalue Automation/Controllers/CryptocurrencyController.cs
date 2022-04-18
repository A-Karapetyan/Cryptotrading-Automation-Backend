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
    [Route("api/[controller]")]
    [ApiController]
    public class CryptocurrencyController : BaseController
    {
        private readonly ICryptocurrencyService currencyService;
        public CryptocurrencyController(ICryptocurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        [HttpGet]
        public async Task<List<CryptoListModel>> GetAll()
        {
            return await currencyService.GetAllCryptos();
        }
    }
}
