using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using HappyHappa.Data.MongoDb.Data;
using System.Configuration;

namespace HappyHappa.Data.MongoDb
{
  public class Repository : IRepository, IDisposable
  {
    public Repository()
    {
      this.Client = new MongoClient(ConfigurationManager.ConnectionStrings["MongoConnectionString"].ConnectionString);
      this.Database = this.Client.GetDatabase(ConfigurationManager.AppSettings["MongoDatabaseName"]);
      Mappings.Init();
    }

    public IMongoClient Client { get; set; }

    public IMongoDatabase Database { get; set; }

    public string OwnerId { get; set; }

    public virtual async Task<IRepoResponse> DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : Entity
    {
      try
      {
        var result =
          await this.Database.GetCollection<T>(Util.GetCollectionName(typeof (T))).DeleteManyAsync(expression);
        return new MongoRepoResponse(result);
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse(ex);
      }
    }

    public virtual async Task<IRepoResponse> DeleteAsync<T>(T item) where T : Entity
    {
      try
      {
        Expression<Func<T, bool>> delExpr = i => i.Id.Equals(item.Id);
        var result = await this.Database.GetCollection<T>(Util.GetCollectionName(typeof (T))).DeleteOneAsync(delExpr);
        return new MongoRepoResponse(result);
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse(ex);
      }
    }

    public async Task<IRepoResponse<IEnumerable<T>>> AllAsync<T>(Expression<Func<T, bool>> expression = null) where T : Entity
    {
      try
      {
        if (expression == null) expression = p => true;

        var result = await this.Database.GetCollection<T>(Util.GetCollectionName(typeof (T))).FindAsync(expression);
        return new MongoRepoResponse<IEnumerable<T>>(result.ToList());
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse<IEnumerable<T>>(ex);
      }
    }

    public virtual async Task<IRepoResponse<T>> SingleAsync<T>(Expression<Func<T, bool>> expression) where T : Entity
    {
      try
      {
        var result = await this.Database
          .GetCollection<T>(Util.GetCollectionName(typeof (T)))
          .AsQueryable()
          .SingleOrDefaultAsync(expression);
        return new MongoRepoResponse<T>(result);
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse<T>(ex);
      }
    }

    public virtual async Task<IRepoResponse<T>> AddAsync<T>(T item) where T : Entity
    {
      try
      {
        await this.Database.GetCollection<T>(Util.GetCollectionName(typeof(T))).InsertOneAsync(item);
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse<T>(ex);
      }

      return new MongoRepoResponse<T>(item);
    }

    public virtual async Task<IRepoResponse> AddAsync<T>(IEnumerable<T> items) where T : Entity
    {
      try
      {
        await this.GetCollection<T>().InsertManyAsync(items);
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse(ex);
      }

      return new MongoRepoResponse(true);
    }

    public async Task<IRepoResponse> UpdateAsync<T>(T item) where T : Entity
    {
      try
      {
        var result = await this.GetCollection<T>().ReplaceOneAsync<T>(s => s.Id.Equals(item.Id), item);
        return new MongoRepoResponse(result);
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse(ex);
      }
    }

    public IRepoResponse<T> Single<T>(Expression<Func<T, bool>> expression) where T : Entity
    {
      try
      {
        var result = this.Database
          .GetCollection<T>(Util.GetCollectionName(typeof(T)))
          .AsQueryable()
          .SingleOrDefault(expression);
        return new MongoRepoResponse<T>(result);
      }
      catch (Exception ex)
      {
        return new MongoRepoResponse<T>(ex);
      }
    }

    #region Implementation of IDisposable

    public void Dispose()
    {
      // nothing to do here
    }

    #endregion

    #region HelperMethods

    private IMongoCollection<T> GetCollection<T>()
    {
      return this.Database.GetCollection<T>(Util.GetCollectionName(typeof(T)));
    }
    #endregion
  }
}
