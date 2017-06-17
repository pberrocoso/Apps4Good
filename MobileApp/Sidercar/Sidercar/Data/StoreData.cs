using Sidercar.SidecarAPIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidercar.Data
{
    public class StoreData
    {
        // this is the default static instance you'd use like string text = Settings.Default.SomeSetting;
        public readonly static StoreData Default = new StoreData();

        public StoreData()
        {
            this.PosicionesAvisos = new List<Aviso>();
        }
        public List<Aviso> PosicionesAvisos { get; set; } // add setting properties as you wish
       
    }
}
