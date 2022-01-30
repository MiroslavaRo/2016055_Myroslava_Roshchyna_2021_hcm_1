using Hcm.Core.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hcm.Database.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : DatabaseModel
    {
        protected DatabaseContext Context { get; }

        protected Repository(DatabaseContext context)
        {
            Context = context;
        }

        public Task<TEntity> GetAsync(string id)
        {
            return Context.Set<TEntity>()
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await Context.Set<TEntity>()
                .ToListAsync();
        }

        public async Task<string> SaveAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            return entity.Id;
        }

        public Task<string> UpdateAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity.Id);
        }

        public Task<string> DeleteAsync(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
            return Task.FromResult(entity.Id);
        }

        public IQueryable<TEntity> Query()
        {
            return Context
                .Set<TEntity>()
                .AsQueryable();
        }
    }
}
