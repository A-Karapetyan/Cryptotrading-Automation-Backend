using CA.DAL.Context;
using CA.DAL.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace ABM.DAL.Repository
{
    public class Repository : IRepository
    {
        private readonly EntityDbContext db;
        public Repository(EntityDbContext context)
        {
            db = context;
        }

        #region Sync
        public IQueryable<T> GetAll<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity
        {
            var set = db.Set<T>().AsQueryable();
            if (includeExpression.Any())
                set = includeExpression.Aggregate(set, (current, variable) => current.Include(variable));
            return set;
        }

        public IQueryable<T> GetAllAsNoTracking<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity
        {
            var set = db.Set<T>().AsNoTracking();
            if (includeExpression.Any())
                set = includeExpression.Aggregate(set, (current, variable) => current.Include(variable));
            return set;
        }

        public IQueryable<T> Filter<T>(Expression<Func<T, bool>> query) where T : BaseEntity
        {
            return db.Set<T>().Where(x => !x.IsDeleted).Where(query);
        }

        public async Task<T> AddAsync<T>(T entity) where T : class, IBaseEntity
        {
            db.WriterSet<T>().Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> FilterWithoutIsDeleted<T>(Expression<Func<T, bool>> query) where T : BaseEntity
        {
            return db.Set<T>().Where(query);
        }

        public IQueryable<T> FilterAsNoTracking<T>(Expression<Func<T, bool>> query,
           params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity
        {
            if (query == null)
                throw new Exception("Query is null");
            var set = db.Set<T>().Where(query).AsNoTracking();
            if (includeExpression.Any())
                set = includeExpression.Aggregate(set, (current, variable) => current.Include(variable));
            return set;
        }

        public bool Update<T>(T entity, ExpandoObject properties) where T : BaseEntity
        {
            if (entity == null)
                return false;
            db.Entry(entity).State = EntityState.Unchanged;
            if (!properties.Any())
                return false;
            db.Set<T>().Update(entity);
            return true;
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            DateTime now = DateTime.UtcNow;
            entity.UpdateDt = now;
            db.Update(entity);
        }
        #endregion

        #region Async
        public async Task<IEnumerable<T>> GetAllAsync<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity
        {
            var set = db.Set<T>().AsQueryable();
            if (includeExpression.Any())
                set = includeExpression.Aggregate(set, (current, variable) => current.Include(variable));
            return await set.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsNoTrackingAsync<T>(params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity
        {
            var set = db.Set<T>().AsNoTracking();
            if (includeExpression.Any())
                set = includeExpression.Aggregate(set, (current, include) => current.Include(include));
            return await set.ToListAsync();
        }

        public async Task<T> FindAsync<T>(int id) where T : BaseEntity
        {
            return await db.FindAsync<T>(id);
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class, IBaseEntity
        {
            var set = db.WriterSet<T>().Where(x => x.Id == id && !x.IsDeleted);
            return await set.FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync<T>(int id) where T : class, IBaseEntity
        {
            try
            {
                var entityToRemove = await GetByIdAsync<T>(id);
                if (entityToRemove == null)
                    return true;
                entityToRemove.IsDeleted = true;
                db.WriterSet<T>().Update(entityToRemove);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<T> GetByIdAsync<T>(long id, params Expression<Func<T, object>>[] includeExpression) where T : BaseEntity
        {
            var set = db.Set<T>().Where(x => x.Id == id);
            if (includeExpression.Any())
                set = includeExpression.Aggregate(set, (current, variable) => current.Include(variable));
            return await set.FirstOrDefaultAsync();
        }

        public async Task<bool> Remove<T>(long id) where T : BaseEntity
        {
            try
            {
                var entityToRemove = await GetByIdAsync<T>(id);
                if (entityToRemove == null)
                    return true;
                entityToRemove.IsDeleted = true;
                db.Set<T>().Update(entityToRemove);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> HardRemove<T>(long id) where T : BaseEntity
        {
            try
            {
                var entityToRemove = await GetByIdAsync<T>(id);
                db.Set<T>().Remove(entityToRemove);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> HardRemoveRange<T>(IList<long> ids) where T : BaseEntity
        {
            try
            {
                var entityToRemove = await Filter<T>(x => ids.Contains(x.Id)).ToListAsync();
                db.Set<T>().RemoveRange(entityToRemove);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<T> CreateAsync<T>(T entity) where T : BaseEntity
        {
            DateTime now = DateTime.UtcNow;
            entity.CreateDt = now;
            entity.UpdateDt = now;
            await db.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<int> SaveChanges()
        {
            return await db.SaveChangesAsync();
        }
        #endregion
        public void Dispose()
        {
            db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}