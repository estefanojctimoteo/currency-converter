﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Currency_Converter.Domain.Interfaces
{
    public interface IRepository { }

    public interface IRepository<TEntity> : IRepository
    {
        void Add (TEntity obj);
        TEntity GetById(Guid id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        void Remove(long id);
        IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate);
    }
}