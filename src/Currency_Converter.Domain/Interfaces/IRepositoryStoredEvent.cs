using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Currency_Converter.Domain.Core.Models;

namespace Currency_Converter.Domain.Interfaces
{
    public interface IRepositoryStoredEvent { }

    public interface IRepositoryStoredEvent<TEntity> : IRepositoryStoredEvent //IDisposable where TEntity : EntityUsuario<TEntity>
    {
        void Adicionar(TEntity obj);
        TEntity ObterPorId(Guid id);
        IEnumerable<TEntity> ObterTodos();
        void Update(TEntity obj);
        void Remover(Guid id);
        IEnumerable<TEntity> Buscar(Expression<Func<TEntity, bool>> predicate);
        //int SaveChanges();
    }
}