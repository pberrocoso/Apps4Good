using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidercar.Data
{
    public class Aviso
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public string Velocidad { get; set; }
        public double Distancia { get; set; }
        public string Via { get; set; }
        public bool Notificar { get; set; }
    }
}
