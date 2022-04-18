using ABM.DAL.Repository;
using CA.DAL.Entity;
using CA.DTO.Models;

namespace CA.BLL.Services
{
    public class SymptomService : ISymptomService
    {
        private readonly IRepository repository;
        private readonly ICriteriaService criteriaService;
        public SymptomService(IRepository repository, ICriteriaService criteriaService)
        {
            this.repository = repository;
            this.criteriaService = criteriaService;
        }
        public bool AddSymptom(SymptomCreateModel model)
        {
            foreach (var item in model.Criterias)
            {
                CriteriaCreateModel criteriaModel = new CriteriaCreateModel();
                criteriaModel.CryptoId = item.CryptoId;
                criteriaModel.Price = item.Price;
                criteriaModel.Operation = item.Operation;
                criteriaService.AddCriteria(criteriaModel);
            }

            Symptom symptom = new Symptom();

            symptom.UserId = model.UserId;
   
            repository.AddAsync(symptom);

            return true;
        }

        public bool EditSymtom(SymptomCreateModel model)
        {
            foreach (var item in model.Criterias)
            {
                CriteriaCreateModel criteriaModel = new CriteriaCreateModel();
                criteriaModel.CryptoId = item.CryptoId;
                criteriaModel.Price = item.Price;
                criteriaModel.Operation = item.Operation;
                criteriaService.AddCriteria(criteriaModel);
            }

            Symptom symptom = new Symptom();

            symptom.UserId = model.UserId;

            repository.Update(symptom);

            return true;
        }
    }
}
