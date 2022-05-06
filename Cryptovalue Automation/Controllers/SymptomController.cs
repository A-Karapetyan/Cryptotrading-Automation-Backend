using CA.BLL.Services;
using CA.DTO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptovalue_Automation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SymptomController : BaseController
    {
        private readonly ISymptomService symptomService;
        private readonly ICriteriaService criteriaService;
        public SymptomController(ISymptomService symptomService, ICriteriaService criteriaService)
        {
            this.symptomService = symptomService;
            this.criteriaService = criteriaService;
        }

        [HttpPost]
        [Authorize]
        public async Task<string> AddSymptom([FromBody] SymptomCreateModel model)
        {
            return await symptomService.AddSymptom(model, GetUserIdFromToken());
        }

        [HttpPost]
        [Authorize]
        public async Task<string> EditSymptomTitle([FromBody] EditSymptomTitleModel model)
        {
            return await symptomService.EditSymptomTitle(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<string> EditCriteria([FromBody] CriteriaEditModel model)
        {
            return await criteriaService.EditCriteria(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<string> DeleteSymptom([FromQuery] int id)
        {
            return await symptomService.DeleteSymptom(id);
        }

        [HttpGet]
        [Authorize]
        public List<SymptomResponseModel> GetUserSymptoms()
        {
            return symptomService.GetSymptomsByUserId(GetUserIdFromToken());
        }
    }
}
