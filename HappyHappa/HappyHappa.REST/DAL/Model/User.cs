﻿using HappyHappa.Data.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyHappa.REST.DAL.Model
{
  public class User : Entity
  {
    public int Id { get; set; }
    public Fridge Fridge { get; set; }
  }
}