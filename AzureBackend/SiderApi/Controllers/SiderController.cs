using SiderAPIService.Infraestructura.Respositorio;
using SiderAPIService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SiderApi.Controllers
{
    public class SiderController : ApiController
    {      

        // POST api/Position?Tipo=1&Latitud=40.443973&Longitud=-3.662304&Velocidad=50
        [Route("api/Position")]
        public IHttpActionResult PostPosition([FromBody] InsModel Posicion)
        {
            APIRepository Repo = new APIRepository();
            Repo.InsertaPosicion(int.Parse(Posicion.idTipo), Posicion.Latitud, Posicion.Longitud, Posicion.Velocidad, string.Empty, Posicion.Identificador);
            return Ok();
        }

        //GET api/Cercanos?Latitud=40.457353&Longitud=-3.588838&metros=5000
        [Route("api/Cercanos")]
        public List<PosModel> GetCercanos (string Latitud, string Longitud, string metros, string tiempo)
        {
            APIRepository Repo = new APIRepository();
            return Repo.getCercanos(Latitud, Longitud, int.Parse(metros), int.Parse(tiempo)).OrderBy(p=>p.Distancia).ToList();
        }
        
    }
}
