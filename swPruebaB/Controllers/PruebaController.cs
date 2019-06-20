using swPruebaB.Services;
using swPruebaB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace swPruebaB.Controllers
{
    public class PruebaController : ApiController
    {

        private PruebaDataAcces pruebaDataAcces;

        public PruebaController()
        {
            this.pruebaDataAcces = new PruebaDataAcces();
        }

        public Prueba[] Get()
        {
            return pruebaDataAcces.GetAllPrueba();
        }


        public HttpResponseMessage Post(Prueba prueba)
        {
            this.pruebaDataAcces.SavePrueba(prueba);

            var response = Request.CreateResponse<Prueba>(System.Net.HttpStatusCode.Created, prueba);

            return response;
        }


    }
}
