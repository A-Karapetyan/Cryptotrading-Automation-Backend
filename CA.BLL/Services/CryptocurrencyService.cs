using ABM.DAL.Repository;
using CA.DAL.Entity;
using CA.DTO.Models;
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
                Price = crypto.Price
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
    }
}
