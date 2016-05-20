using System;
using System.Collections.Generic;
using MongoDB.Driver;
using HappyHappa.Data.MongoDb.Data;

namespace HappyHappa.Data.MongoDb
{
  public class MongoRepoResponse : IRepoResponse
  {
    public MongoRepoResponse()
    {
    }

    public MongoRepoResponse(DeleteResult deleteResult)
    {
      this.Success = deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
      if (deleteResult.DeletedCount == 0)
      {
        this.Errors = new List<string>() { "no matching entries found" };
      }
    }

    public MongoRepoResponse(Exception exception)
    {
      this.Success = false;
      this.Errors = new List<string>() { exception.Message };
    }

    public MongoRepoResponse(bool result)
    {
      this.Success = result;
    }

    public MongoRepoResponse(ReplaceOneResult replaceResult)
    {
      if (replaceResult.IsAcknowledged)
      {
        if (replaceResult.MatchedCount == 0)
        {
          this.Success = false;
          this.Errors = new List<string>() { "no matching entries found" };
        }
        else if (replaceResult.ModifiedCount != 1)
        {
          this.Success = false;
          this.Errors = new List<string>() { "entry not changed" };
        }
        else
        {
          this.Success = true;
        }
      }
      else
      {
        this.Success = false;
        this.Errors = new List<string>() { "not acknowledged" };
      }
    }

    public bool Success { get; private set; }
    public IEnumerable<string> Errors { get; set; }
  }

  public class MongoRepoResponse<T> : IRepoResponse<T>
  {
    public MongoRepoResponse(T result)
    {
      if (result != null)
      {
        this.Data = result;
        this.Success = true;
      }
      else
      {
        this.Success = false;
        this.Errors = new List<string>() { "no results found" };
      }
    }

    public MongoRepoResponse(Exception ex)
    {
      this.Success = false;
      this.Errors = new List<string>() { ex.Message };
    }

    public T Data { get; private set; }
    public bool Success { get; }
    public IEnumerable<string> Errors { get; }
  }
}
