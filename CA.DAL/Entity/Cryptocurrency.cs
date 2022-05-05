using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DAL.Entity
{
    public class Cryptocurrency : BaseEntity
    {
        public ICollection<Criteria> Criterias { get; set; }
        public string Name { get; set; }
        public string CoinbaseId { get; set; }
        public string Image { get; set; }
        public bool Published { get; set; }
        public string Currency { get; set; }
        public decimal? Price { get; set; }
        public int? MainCurrencyId { get; set; }
        public ICollection<History> Histories { get; set; }
    }
}
