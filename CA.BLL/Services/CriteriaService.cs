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
            Criteria criteria = new Criteria();
            criteria.Price = model.Price;
            criteria.Operation = model.Operation;
            criteria.CryptoId = model.CryptoId;

            repository.AddAsync(criteria);
        }
    }
}
