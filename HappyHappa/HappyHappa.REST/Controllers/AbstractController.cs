using HappyHappa.REST.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HappyHappa.REST.Controllers
{
  public abstract class AbstractController : ApiController
  {
    public IDAL Dal { get; set; }

    public AbstractController(IDAL dal) {
      Dal = dal;
    }
  }
}