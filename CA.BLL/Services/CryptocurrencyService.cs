using ABM.DAL.Repository;
using CA.DAL.Entity;
using CA.DTO.Models;
using CA.Infrastucture.Enums;
using CA.Infrastucture.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.BLL.Services
{
    public class CryptocurrencyService : ICryptocurrencyService
    {
        private readonly IRepository repository;
        public CryptocurrencyService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<CryptoListModel>> GetAllCryptos()
        {
            return await repository.GetAllAsNoTracking<Cryptocurrency>().Select(c => new CryptoListModel
            {
                Image = c.Image,
                Name = c.Name,
                Id = c.Id
            }).ToListAsync();
        }

        public async Task<CryptoDetailModel> GetById(int id)
        {
            var crypto = await repository.Filter<Cryptocurrency>(c => c.Id == id).Include(cr => cr.Histories).FirstOrDefaultAsync();
            var result = new CryptoDetailModel
            {
                Image = crypto.Image,
                Name = crypto.Name,
                Price = crypto.Price,
                Id = crypto.Id
            };

            if (crypto.Histories != null)
            {
                List<CryptoHistoryModel> histories = new List<CryptoHistoryModel>();

                foreach (var hy in crypto.Histories)
                {
                    histories.Add(new CryptoHistoryModel
                    {
                        Date = hy.Date,
                        Price = hy.Price
                    });
                }

                result.Histories = histories;
            }

            return result;
        }

        public async Task<bool> UpdateCryptoData()
        {
            try
            {
                var res = await HttpClientHelper.GetRequest<CryptosListResModel>("https://api.coinbase.com/v2/exchange-rates?currency=USD");
                var cryptocurrencies = await repository.GetAll<Cryptocurrency>().Include(c => c.Histories).Include(c => c.Criterias).ThenInclude(cr => cr.Symptom).ToListAsync();
                var currencies = res.data.rates;

                foreach (var cryptocurrency in cryptocurrencies)
                {
                    object price;
                    if (cryptocurrency.Currency == "1INCH")
                    {
                        price = currencies.GetType().GetProperties().Where(p => p.Name == "_1INCH").FirstOrDefault().GetValue(currencies);
                    }
                    else
                    {
                        price = currencies.GetType().GetProperties().Where(p => p.Name == cryptocurrency.Currency).FirstOrDefault().GetValue(currencies);
                    }
                    cryptocurrency.Price = 1 / Convert.ToDecimal(price.ToString());

                    History history = new History
                    {
                        Date = DateTime.Now,
                        Price = cryptocurrency.Price
                    };

                    cryptocurrency.Histories.Add(history);
                }

                await repository.SaveChanges();

                var symptoms = repository.GetAll<Symptom>().Include(s => s.User).Include(s => s.Criterias).ThenInclude(cr => cr.Crypto).Where(c => 
                c.Criterias.Any(c => (c.Crypto.Price > c.Price && c.Operation == CriteriaOperationEnum.Greater) || (c.Crypto.Price < c.Price && c.Operation == CriteriaOperationEnum.Lower))).ToList();

                foreach (var symptom in symptoms)
                {
                    await new MailHelper().SendEmail(symptom.User.Email, "The symptom you created is now valid", $"Title <b>{symptom.Title}</b>");
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
