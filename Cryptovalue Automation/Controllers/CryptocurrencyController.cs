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
    [Route("api/[controller]/[action]")]
    public class CryptocurrencyController : BaseController
    {
        private readonly ICryptocurrencyService cryptoCurrencyService;
        private readonly IHistoryService hisoryService;
        public CryptocurrencyController(ICryptocurrencyService cryptoCurrencyService, IHistoryService hisoryService)
        {
            this.cryptoCurrencyService = cryptoCurrencyService;
            this.hisoryService = hisoryService;
        }

        [HttpGet]
        public async Task<List<CryptoListModel>> GetAll()
        {
            return await cryptoCurrencyService.GetAllCryptos();
        }

        [HttpGet]
        [Authorize]
        public async Task<CryptoDetailModel> GetById([FromQuery] int id)
        {
            return await cryptoCurrencyService.GetById(id);
        }

        [HttpGet]
        public async Task<bool> UpdateCryptoData()
        {
            return await cryptoCurrencyService.UpdateCryptoData();
        }

        [HttpGet]
        public async Task<bool> UpdateCryptoJob()
        {
            RecurringJob.AddOrUpdate(() => UpdateCryptoData(), Cron.MinuteInterval(10));
            return true;
        }

        [HttpGet]
        public async Task<bool> DeleteAllHistories()
        {
            return await hisoryService.DeleteAll();
        }
    }
}
