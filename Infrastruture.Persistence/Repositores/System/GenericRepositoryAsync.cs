using Core.Application.Interfaces.System;
using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastruture.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Infrastruture.Persistence.Repositores.System
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        public readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public virtual async Task<T> GetByGUIDAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entitys)
        {

            foreach (var n in entitys)
            {
                await _dbContext.Set<T>().AddAsync(n);
            }
            await _dbContext.SaveChangesAsync();
            return entitys;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public async Task UpdateAsync(Action<T> setter, Func<T, bool> equater)
        {
            var enumerable = _dbContext.Set<T>()?.Where(equater)?.ToList();
            if (enumerable is null || enumerable.Count < 1) return;
            foreach (var item in enumerable)
            {
                setter(item);
                _dbContext.Entry(item).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Func<T, bool> equater)
        {
            var enumerable = _dbContext.Set<T>()?.Where(equater)?.ToList();
            if (enumerable is null || enumerable.Count < 1) return;
            foreach (var item in enumerable)
            {
                _dbContext.Set<T>().Remove(item);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> equater)
        {
            return await _dbContext.Set<T>().Where(equater).FirstOrDefaultAsync();
        }
    }
}
