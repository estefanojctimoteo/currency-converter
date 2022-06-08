using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Currency_Converter.Domain.Core.Models;
using Currency_Converter.Domain.Interfaces;
using Currency_Converter.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Currency_Converter.Infra.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<TEntity>
    {
        protected readonly ConversionContext Db;
        protected Repository(ConversionContext context)
        {
            Db = context;            
        }

        public virtual void Add(TEntity obj)
        {
            Db.Set<TEntity>().Add(obj);
            Save();
        }

        public virtual void Update(TEntity obj)
        {
            Db.Entry(obj).State = EntityState.Modified;
            Save();
        }

        public virtual IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate)
        {
            return Db.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity GetById(Guid id)
        {
            return Db.Set<TEntity>().AsNoTracking().FirstOrDefault(t => t.Id == id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Db.Set<TEntity>().ToList();
        }

        public virtual void Remove(long id)
        {            
            var obj = Db.Set<TEntity>().Find(id);
            Db.Entry(obj).State = EntityState.Deleted;
            Save();
        }

        private void Save()
        {            
            try
            {
                Db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                var sb = new StringBuilder();
              
                sb.AppendLine($"Error: technical detail::: {e?.InnerException?.InnerException?.Message}");

                foreach (var eve in e.Entries)
                {
                    sb.AppendLine($"An object [{eve.Entity.GetType().Name}] with the state [{eve.State}] can not be updated.");
                }
            }
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}