using CA.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.BLL.Services
{
    public interface ISymptomService
    {
        bool AddSymptom(SymptomCreateModel model);
        bool EditSymtom(SymptomCreateModel model);
        List<SymptomResponseModel> GetSymptomsByUserId(int userId);
    }
}
