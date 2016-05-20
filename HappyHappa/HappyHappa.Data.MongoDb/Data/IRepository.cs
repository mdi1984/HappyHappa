using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HappyHappa.Data.MongoDb.Data
{
  public interface IRepository
  {
    string OwnerId { get; set; }
    Task<IRepoResponse> DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : Entity;
    Task<IRepoResponse> DeleteAsync<T>(T item) where T : Entity;
    Task<IRepoResponse<IEnumerable<T>>> AllAsync<T>(Expression<Func<T, bool>> expression = null) where T : Entity;
    Task<IRepoResponse<T>> SingleAsync<T>(Expression<Func<T, bool>> expression) where T : Entity;
    Task<IRepoResponse<T>> AddAsync<T>(T item) where T : Entity;
    Task<IRepoResponse> AddAsync<T>(IEnumerable<T> items) where T : Entity;
    Task<IRepoResponse> UpdateAsync<T>(T item) where T : Entity;
    IRepoResponse<T> Single<T>(Expression<Func<T, bool>> expression) where T : Entity;

  }
}