using Microsoft.VisualStudio.TestTools.UnitTesting;
using SiderAPIService.Infraestructura.Respositorio;
using SiderAPIService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiderAPIService.Infraestructura.Respositorio.Tests
{
    [TestClass()]
    public class GoogleAPIRepositoryTests
    {
        [TestMethod()]
        public void getDatosRutaTest()
        {
            GoogleAPIRepository repo = new GoogleAPIRepository();
            string APIGoogleKEY = System.Configuration.ConfigurationManager.AppSettings["APIGoogleKEY"];
            GeoCodeModel model = repo.getDatosRuta("39.706181,%20-6.356843", APIGoogleKEY);

            var result = from results in model.results
                         from addrs in results.address_components                        
                         where addrs.types.Contains("route")
                         select addrs;



           Assert.IsNotNull(result);
        }
    }
}