using SiderAPIService.Infraestructura.DataModel;
using SiderAPIService.Infraestructura.Querys;
using SiderAPIService.Model;
using SiderAPIService.Servicio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiderAPIService.Infraestructura.Respositorio
{
    public class APIRepository
    {
        public List<PosModel> getCercanos(string Latitud, string Longitud, int Metros, int Tiempo)
        {

            List<PosModel> ListadoVacio = new List<PosModel>();
            Latitud = Latitud.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            Longitud = Longitud.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            decimal dlalitud = decimal.Parse(Latitud);
            decimal dlongitud = decimal.Parse(Longitud);

            using (siderSQLEntities bbdd = new DataModel.siderSQLEntities())
            {

                var ListaResultado = bbdd.GET_DATOS_POSICIONES(dlalitud, dlongitud, Metros, Tiempo).ToList();

                if (ListaResultado.Count() > 0)
                {
                    var listadoDevolver = (from r in ListaResultado
                                           select new PosModel()
                                           {
                                               Latitud = r.Latitud,
                                               Longitud = r.Longitud,
                                               Distancia = r.DISTANCIA.Value,
                                               Velocidad = r.Velocidad.Value,
                                               Tipo = r.Tipo,
                                               Via = r.Via,
                                               id = r.Usuario
                                           }).ToList();

                    return listadoDevolver.DistinctBy(p => p.id).ToList();

                }
                else
                {
                    return ListadoVacio;
                }
            }
        }       

        private int metrosSegundoKilometrosHora(string velocidad)
        {
            int iResult = 0;
            try
            {
                velocidad = velocidad.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                decimal dVelocidad = decimal.Parse(velocidad);

                dVelocidad = ((dVelocidad * 3600) / 1000);
                dVelocidad = Math.Truncate(dVelocidad);
                iResult = int.Parse(dVelocidad.ToString());
            }
            catch (Exception)
            {
                iResult = -1;
            }

            return iResult;
        }

        public Resultado InsertaPosicion(int idTipo, string Latitud, string Longitud, string Velocidad, string route, string usuario)
        {
            using (siderSQLEntities bbdd = new DataModel.siderSQLEntities())
            {

                Resultado resultado = new Model.Resultado();
                resultado.STATUS = Constantes.RESULTOK;

                Latitud = Latitud.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                Longitud = Longitud.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                Posiciones elemento = new DataModel.Posiciones();
                elemento.IdTipo = idTipo;
                elemento.Latitud = decimal.Parse(Latitud);
                elemento.Longitud = decimal.Parse(Longitud);
                elemento.Velocidad = metrosSegundoKilometrosHora(Velocidad);
                elemento.Via = GetRuta(Latitud.Replace(",", ".") + "," + Longitud.Replace(",", "."));
                elemento.Usuario = usuario;
                elemento.fecha = DateTime.Now;
                elemento.idEstado = 1; //De momento por el API sólo se puede generar un elemento temporal
                bbdd.Posiciones.Add(elemento);
                bbdd.SaveChanges();

                return resultado;
            }
        }

        private string GetRuta(string ltnlgn)
        {
            GoogleAPIRepository repo = new GoogleAPIRepository();
            string APIGoogleKEY = System.Configuration.ConfigurationManager.AppSettings["APIGoogleKEY"];
            GeoCodeModel model = repo.getDatosRuta(ltnlgn, APIGoogleKEY);

            var result = from results in model.results
                         from addrs in results.address_components
                         where addrs.types.Contains("route")
                         select addrs;

            if (result != null)
            {
                return result.First().short_name;
            }

            return string.Empty;
        }
    }
}
