using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CA.DAL.Entity;

namespace ABM.DAL.Repository
{
    public interface IRepository
    {
        #region Sync
        IQueryable<T> GetAll<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity;
        IQueryable<T> GetAllAsNoTracking<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity;
        IQueryable<T> Filter<T>(Expression<Func<T, bool>> query) where T : BaseEntity;
        IQueryable<T> FilterWithoutIsDeleted<T>(Expression<Func<T, bool>> query) where T : BaseEntity;
        IQueryable<T> FilterAsNoTracking<T>(Expression<Func<T, bool>> query, params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity;
        bool Update<T>(T entity, ExpandoObject properties) where T : BaseEntity;
        void Update<T>(T entity) where T : BaseEntity;
        #endregion

        #region Async
        Task<T> FindAsync<T>(int id) where T : BaseEntity;
        Task<IEnumerable<T>> GetAllAsync<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity;
        Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity;
        Task<bool> Remove<T>(long id) where T : BaseEntity;
        Task<bool> HardRemove<T>(long id) where T : BaseEntity;
        Task<bool> HardRemoveRange<T>(IList<long> ids) where T : BaseEntity;
        Task<T> CreateAsync<T>(T entity) where T : BaseEntity;
        Task<T> AddAsync<T>(T entity) where T : class, IBaseEntity;
        Task<T> GetByIdAsync<T>(long id, params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity;
        Task<T> GetByIdAsync<T>(int id) where T : class, IBaseEntity;
        Task<bool> DeleteAsync<T>(int id) where T : class, IBaseEntity;
        Task<int> SaveChanges();
        #endregion
        void Dispose();
    }
}