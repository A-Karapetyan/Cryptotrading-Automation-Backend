using CA.Infrastucture.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DTO.Models
{
    public class CriteriaCreateModel
    {
        public decimal Price { get; set; }
        public int CryptoId { get; set; }
        public CriteriaOperationEnum Operation { get; set; }
        public int SymptomId { get; set; }
    }
}
