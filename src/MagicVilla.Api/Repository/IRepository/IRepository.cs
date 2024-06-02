﻿using System.Linq.Expressions;

namespace MagicVilla.Api.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task SaveAsync();
    }
}
