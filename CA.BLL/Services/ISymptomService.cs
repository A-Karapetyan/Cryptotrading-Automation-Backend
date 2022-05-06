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
        Task<string> AddSymptom(SymptomCreateModel model, int userId);
        Task<string> EditSymptomTitle(EditSymptomTitleModel model);
        Task<string> DeleteSymptom(int symptomId);
        List<SymptomResponseModel> GetSymptomsByUserId(int userId);
    }
}
