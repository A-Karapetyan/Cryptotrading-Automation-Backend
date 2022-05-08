using ABM.DAL.Repository;
using CA.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.BLL.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IRepository repository;
        public HistoryService(IRepository repository)
        {
            this.repository = repository;
        }
        public async Task<bool> DeleteAll()
        {
            try
            {
                IList<long> ids = new List<long>();
                var histories = repository.GetAll<History>().ToList();
                foreach (var history in histories)
                {
                    ids.Add(history.Id);
                }
                bool result = await repository.HardRemoveRange<History>(ids);
                await repository.SaveChanges();
                return result;
            }
            catch(Exception e)
            {
                return false;
            }
      
        }
    }
}
