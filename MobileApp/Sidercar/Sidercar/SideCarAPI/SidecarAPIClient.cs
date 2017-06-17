using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidercar.SidecarAPIModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace Sidercar.SideCarAPI
{
    public class SidecarAPIClient
    {
        public async Task<List<PosModel>> getDatosPosCercanasAsync(string Latitude, string Longitude, string metros, string tiempo)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var json = await client.GetStringAsync(string.Format("http://sidermobile.azurewebsites.net/api/Cercanos?Latitud={0}&Longitud={1}&metros={2}&tiempo={3}", Latitude, Longitude, metros, tiempo));
                    List<PosModel> PosCercanas = JsonConvert.DeserializeObject<List<PosModel>>(json);
                    return PosCercanas;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task insertaDatosAPI(InsModel mod)
        {
            try
            {
                using (var client = new HttpClient())
                {                    
                    var json = JsonConvert.SerializeObject(mod);
                    var stringContent = new StringContent(json,
                         UnicodeEncoding.UTF8,
                         "application/json"); 

                    client.BaseAddress = new Uri("http://sidermobile.azurewebsites.net");
                    var response = await client.PostAsync("api/Position", stringContent);

                }
            }
            catch (Exception ex)
            {
               ///TODO: Control de excepciones
            }
        }

    }
}
