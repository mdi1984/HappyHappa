using System.Collections.Generic;

namespace HappyHappa.Data.MongoDb.Data
{
  public interface IRepoResponse
  {
    bool Success { get; }
    IEnumerable<string> Errors { get; }
  }

  public interface IRepoResponse<out T> : IRepoResponse
  {
    T Data { get; }
  }
}