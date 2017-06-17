using Newtonsoft.Json;
using SiderAPIService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SiderAPIService.Infraestructura.Respositorio
{
    public class GoogleAPIRepository
    {
        //https://maps.googleapis.com/maps/api/geocode/json?latlng={0}&result_type=route|street_address&key=AIzaSyCBiUtcahgL_ugrBcy6PjT9RsBnrTQzK2k


        public GeoCodeModel getDatosRuta(string latlng,string KEY)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0}&result_type=route|street_address&key={1}", latlng, KEY)).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    GeoCodeModel GeoResult = JsonConvert.DeserializeObject<GeoCodeModel>(json);
                    return GeoResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
