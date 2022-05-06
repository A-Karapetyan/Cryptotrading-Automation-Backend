using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.DTO.Models
{
    public class SymptomCreateModel
    {
        public List<CriteriaCreateModel> Criterias { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
    }
}
