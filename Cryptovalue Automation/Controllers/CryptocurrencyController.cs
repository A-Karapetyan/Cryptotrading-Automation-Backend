using CA.BLL.Services;
using CA.DTO.Models;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public CryptoDetailModel GetById([FromQuery] int id)
        {
            return cryptoCurrencyService.GetById(id);
        }

        [HttpGet]
        public async Task<bool> UpdateCryptoData()
        {
            return await cryptoCurrencyService.UpdateCryptoData();
        }

        [HttpGet]
        public async Task<bool> UpdateCryptoJob()
        {
            RecurringJob.AddOrUpdate(() => UpdateCryptoData(), Cron.MinuteInterval(1));
            return true;
        }
    }
}
