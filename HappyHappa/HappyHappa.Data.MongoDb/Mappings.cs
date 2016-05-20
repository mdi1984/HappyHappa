using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace HappyHappa.Data.MongoDb
{
  public static class Mappings
  {
    private static bool initialized;

    public static void Init()
    {
      if (!initialized) RegisterClassMaps();
    }

    private static void RegisterClassMaps()
    {
      if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
      {
        BsonClassMap.RegisterClassMap<Entity>(cm =>
        {
          cm.MapIdProperty(e => e.Id)
            .SetIdGenerator(StringObjectIdGenerator.Instance)
            .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });
      }

      initialized = true;
    }
  }
}
