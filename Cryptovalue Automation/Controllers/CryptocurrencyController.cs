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
    public class CryptocurrencyController : BaseController
    {
        private readonly ICryptocurrencyService cryptoCurrencyService;
        public CryptocurrencyController(ICryptocurrencyService cryptoCurrencyService)
        {
            this.cryptoCurrencyService = cryptoCurrencyService;
        }

        [HttpGet]
        public async Task<List<CryptoListModel>> GetAll()
        {
            return await cryptoCurrencyService.GetAllCryptos();
        }

        [HttpGet]
        public CryptoDetailModel GetById([FromQuery] int id)
        {
            return cryptoCurrencyService.GetById(id);
        }
    }
}
