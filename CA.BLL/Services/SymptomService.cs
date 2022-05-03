using ABM.DAL.Repository;
using CA.DAL.Entity;
using CA.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            try
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
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool EditSymtom(SymptomCreateModel model)
        {
            try
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
            } 
            catch(Exception e)
            {
                return false;
            }
         

            return true;
        }

        public List<SymptomResponseModel> GetSymptomsByUserId(int userId)
        {
            List<SymptomResponseModel> result = new List<SymptomResponseModel>();
            IQueryable<Symptom> symptoms = repository.GetAllAsNoTracking<Symptom>(s => s.UserId == userId);

            foreach(var symptom in symptoms)
            {
                SymptomResponseModel symptomModel = new SymptomResponseModel();
                List<CriteriaResponseModel> criterias = new List<CriteriaResponseModel>();

                foreach(Criteria criteria in symptom.Criterias)
                {
                    CriteriaResponseModel model = new CriteriaResponseModel();

                    model.Crypto = new CryptoListModel();

                    model.Price = criteria.Price;
                    model.Operation = criteria.Operation;
                    model.Crypto.Image = criteria.Crypto.Image;
                    model.Crypto.Name = criteria.Crypto.Name;

                    criterias.Add(model);
                }

                symptomModel.Criterias = criterias;
                symptomModel.Title = symptom.Title;
                result.Add(symptomModel);
            }

            return result; 
        }
    }
}
