using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Model
{
  public class EvaluatedRecipe
  {
    public Recipe Recipe { get; set; }
    public double Evaluation { get; set; }
  }
}