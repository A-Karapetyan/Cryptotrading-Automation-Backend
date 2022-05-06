using ABM.DAL.Repository;
using CA.DAL.Entity;
using CA.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.BLL.Services
{
    public class CriteriaService : ICriteriaService
    {
        private readonly IRepository repository;
        public CriteriaService(IRepository repository)
        {
            this.repository = repository;
        }
        public void AddCriteria(CriteriaCreateModel model)
        {
            try
            {
                Criteria criteria = new Criteria();
                criteria.Price = model.Price;
                criteria.Operation = model.Operation;
                criteria.CryptoId = model.CryptoId;
                criteria.SymptomId = model.SymptomId;

                repository.AddAsync(criteria);
            }
            catch(Exception e)
            {
                throw new Exception("Failed to create new criteria");
            }
        }

        public async Task<string> EditCriteria(CriteriaEditModel model)
        {
            try
            {
                Criteria criteria = repository.Filter<Criteria>(c => c.Id == model.CriteriaId).FirstOrDefault();

                criteria.Operation = model.Operation;
                criteria.Price = model.Price;
                criteria.CryptoId = model.CryptoId;
                await repository.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception("Failed to create new criteria");
            }

            return "ok";
        }
    }
}
