using HappyHappa.Data.MongoDb.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HappyHappa.Data.MongoDb
{
  internal static class Util
  {
    public static string GetCollectionName(Type entityType)
    {
      var nameAttribute = Attribute.GetCustomAttribute(entityType, typeof(CollectionNameAttribute)) as CollectionNameAttribute;
      if (nameAttribute != null)
      {
        return nameAttribute.Name;
      }

      return entityType.Name;
    }
  }

  public class CombineVisitor : ExpressionVisitor
  {
    private readonly Expression from, to;
    public CombineVisitor(Expression from, Expression to)
    {
      this.from = from;
      this.to = to;
    }

    public override Expression Visit(Expression node)
    {
      return node == from ? to : base.Visit(node);
    }
  }
}
