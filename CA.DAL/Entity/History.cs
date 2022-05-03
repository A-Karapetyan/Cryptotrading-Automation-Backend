using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DAL.Entity
{
    public class History : BaseEntity
    {
        public int CryptocurrencyId { get; set; }
        public Cryptocurrency Cryptocurrency { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
