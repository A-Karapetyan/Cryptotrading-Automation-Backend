using CA.BLL.Services;
using CA.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptovalue_Automation.Controllers
{
    public class SymptomController : BaseController
    {
        private readonly ISymptomService symptomService;
        public SymptomController(ISymptomService symptomService)
        {
            this.symptomService = symptomService;
        }

        [HttpPost]
        public async Task<bool> AddSymptom([FromBody]SymptomCreateModel model)
        {
            return symptomService.AddSymptom(model);
        }

        [HttpPost]
        public async Task<bool> EditSymtom([FromBody] SymptomCreateModel model)
        {
            return symptomService.EditSymtom(model);
        }
    }
}
