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

        public CryptoDetailModel GetById(int id)
        {
            var crypto = repository.GetByIdAsync<Cryptocurrency>(id).Result;
            var result = new CryptoDetailModel
            {
                Image = crypto.Image,
                Name = crypto.Name,
                Price = crypto.Price,
                Id = crypto.Id
            };

            if (crypto.Histories != null)
            {
                foreach (var hy in crypto.Histories)
                {
                    result.Histories.Add(new CryptoHistoryModel
                    {
                        Date = hy.Date,
                        Price = hy.Price
                    });
                }
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
                var users = repository.GetAll<User>().Include(u => u.Symptoms).ToList();

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
                    cryptocurrency.Price = 1 / Convert.ToDecimal(price.ToString().Replace(".", ","));

                    History history = new History
                    {
                        Date = DateTime.Now,
                        Price = cryptocurrency.Price
                    };

                    cryptocurrency.Histories.Add(history);
                }

                await repository.SaveChanges();

                foreach (var user in users)
                {
                    foreach (var symptom in user.Symptoms)
                    {
                        if (CheckSymptom(symptom, currencies))
                        {
                            await new MailHelper().SendEmail(user.Email, "The symptom you created is now valid", $"Title <b>{symptom.Title}</b>");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private bool CheckSymptom(Symptom symptom, Rates currencies)
        {
            foreach (var criteria in symptom.Criterias)
            {
                bool isValid = true;

                object price;

                if (criteria.Crypto.Name.ToUpper().Contains("1INCH"))
                {
                    price = currencies.GetType().GetProperties().Where(p => p.Name == "_1INCH").FirstOrDefault().GetValue(currencies);
                }
                else
                {
                    price = currencies.GetType().GetProperties().Where(p => criteria.Crypto.Name.Contains(p.Name)).FirstOrDefault().GetValue(currencies);
                }

                if ((criteria.Price > Convert.ToDecimal(price) && criteria.Operation == CriteriaOperationEnum.Greater) || (criteria.Price < Convert.ToDecimal(price) && criteria.Operation == CriteriaOperationEnum.Lower))
                {
                    continue;
                } else
                {
                    return false;
                }

            }

            return true;
        }
    }
}
