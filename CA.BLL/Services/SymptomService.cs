using ABM.DAL.Repository;
using CA.DAL.Entity;
using CA.DTO.Models;
using Microsoft.EntityFrameworkCore;
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
        public async Task<string> AddSymptom(SymptomCreateModel model, int userId)
        {
            try
            {
                Symptom symptom = new Symptom();
                symptom.Title = model.Title;
                symptom.UserId = userId;

                await repository.AddAsync(symptom);
                await repository.SaveChanges();

                foreach (var item in model.Criterias)
                {
                    Criteria criteriaModel = new Criteria();
                    criteriaModel.CryptoId = item.CryptoId;
                    criteriaModel.Price = item.Price;
                    criteriaModel.Operation = item.Operation;
                    criteriaModel.SymptomId = symptom.Id;
                    await repository.AddAsync(criteriaModel);
                }

                await repository.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "ok";
        }

        public async Task<string> EditSymptomTitle(EditSymptomTitleModel model)
        {
            try
            {
                Symptom symptom = repository.Filter<Symptom>(s => s.Id == model.SymptomId).FirstOrDefault();

                if (symptom != null)
                {
                    symptom.Title = model.Title;
                }

                await repository.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "ok";
        }

        public async Task<string> DeleteSymptom(int symptomId)
        {
            try
            {
                await repository.HardRemove<Symptom>(symptomId);
                await repository.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Symptom does not exist");
            }

            return "ok";
        }

        public List<SymptomResponseModel> GetSymptomsByUserId(int userId)
        {
            List<SymptomResponseModel> result = new List<SymptomResponseModel>();
            var user = repository.Filter<User>(u => u.Id == userId).Include(us => us.Symptoms).ThenInclude(s => s.Criterias).ThenInclude(c => c.Crypto).FirstOrDefault();

            foreach(var symptom in user.Symptoms)
            {
                SymptomResponseModel symptomModel = new SymptomResponseModel();
                List<CriteriaResponseModel> criterias = new List<CriteriaResponseModel>();

                foreach(Criteria criteria in symptom.Criterias)
                {
                    CriteriaResponseModel model = new CriteriaResponseModel();

                    model.Crypto = new CryptoListModel();

                    model.Price = criteria.Price;
                    model.Operation = criteria.Operation;
                    model.Id = criteria.Id;
                    model.Crypto.Image = criteria.Crypto.Image;
                    model.Crypto.Name = criteria.Crypto.Name;
                    model.Crypto.Id = criteria.Crypto.Id;

                    criterias.Add(model);
                }

                symptomModel.Criterias = criterias;
                symptomModel.Title = symptom.Title;
                symptomModel.Id = symptom.Id;
                result.Add(symptomModel);
            }

            return result; 
        }
    }
}
