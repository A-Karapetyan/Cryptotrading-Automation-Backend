using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DTO.Models
{
    public class CryptoDetailModel
    {
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
        public List<CryptoHistoryModel> Histories { get; set; }
    }
}
