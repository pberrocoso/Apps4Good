using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiderAPIService.Model
{
    public class PosModel
    {
        public string Tipo { get; set; }
        public string Via { get; set; }
        public Nullable<decimal> Latitud { get; set; }
        public Nullable<decimal> Longitud { get; set; }
        public double Distancia { get; set; }
        public int Velocidad { get; set; }
        public string id { get; set; }
                
    }
}
