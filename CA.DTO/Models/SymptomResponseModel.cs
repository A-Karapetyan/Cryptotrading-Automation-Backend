using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DTO.Models
{
    public class SymptomResponseModel
    {
        public List<CriteriaResponseModel> Criterias { get; set; }
        public string Title { get; set; }
    }
}
