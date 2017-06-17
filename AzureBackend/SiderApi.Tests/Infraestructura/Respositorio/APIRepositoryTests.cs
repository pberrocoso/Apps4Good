using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiderAPIService.Infraestructura.Respositorio;
using SiderAPIService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiderAPIService.Infraestructura.Respositorio.Tests
{
    [TestClass()]
    public class APIRepositoryTests
    {
        [TestMethod()]
        public void getCercanosTest()
        {
            APIRepository repo = new APIRepository();
            var listado = repo.getCercanos("40.457353", "-3.588838", 4000, 70000);
            Assert.IsNotNull(listado);
        }

        [TestMethod()]
        public void InsertaPosicionTest()
        {
            //    APIRepository repo = new APIRepository();
            //    var listado = repo.InsertaPosicion(1, "40.403912", "-3.652834", "30", string.Empty, "Perico");
            //    Assert.IsNotNull(listado);


            try
            {
                using (var client = new HttpClient())
                {
                    InsModel mod = new InsModel();
                    mod.Identificador = Guid.NewGuid().ToString();
                    mod.Velocidad = "33";
                    mod.idTipo = "1";
                    mod.Latitud = "41.403912";
                    mod.Longitud = "-3.252834";

                    //client.BaseAddress = new Uri("http://sidermobile.azurewebsites.net");
                    client.BaseAddress = new Uri("http://localhost:1745");
                    var response = client.PostAsJsonAsync("api/Position", mod).Result;                                                           
                    Assert.IsNotNull(null);
                }
            }
            catch (Exception ex)
            {
                
            }

            Assert.IsNotNull(null);
        }
    }
}