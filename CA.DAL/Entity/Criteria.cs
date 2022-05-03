using CA.Infrastucture.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DAL.Entity
{
    public class Criteria : BaseEntity
    {
        public decimal Price { get; set; }
        public Cryptocurrency Crypto { get; set; }
        public int CryptoId { get; set; }
        public Symptom Symptom { get; set; }
        public int SymptomId { get; set; }
        public CriteriaOperationEnum Operation { get; set; }
    }
}
