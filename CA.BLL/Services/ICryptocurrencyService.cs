using CA.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.BLL.Services
{
    public interface ICryptocurrencyService
    {
        Task<List<CryptoListModel>> GetAllCryptos();
    }
}
