using System;

namespace HappyHappa.Data.MongoDb.Data
{
  [AttributeUsage(AttributeTargets.Class, Inherited = true)]
  public class CollectionNameAttribute : Attribute
  {
    public CollectionNameAttribute(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
      {
        throw new ArgumentException("non-Empty collectionname required");
      }
      this.Name = name;
    }

    public virtual string Name { get; private set; }
  }
}
