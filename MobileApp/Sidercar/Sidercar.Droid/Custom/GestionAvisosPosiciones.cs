using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Sidercar.SidecarAPIModel;
using Sidercar.Data;

namespace Sidercar.Droid.Custom
{
    public static class GestionAvisosPosiciones
    {
        public static List<Aviso> GetAvisosPosicionesCercanas(List<PosModel> ListaPosiciones)
        {
            foreach (PosModel item in ListaPosiciones)
            {
                Aviso AvisoItem = StoreData.Default.PosicionesAvisos.FirstOrDefault(p => p.Id == item.Id);
                if (AvisoItem != null)
                {
                    //Vamos a comprobar si emitimos aviso o no
                    //Primero, evaluamos si está entre 500 y 1000 metros
                    if (item.Distancia > 500 && item.Distancia <= 1000)
                    {
                        //Actualizamos Aviso
                        UpdateAviso(AvisoItem, item, CalcularConDistanciaAnterior(AvisoItem.Distancia, 500, 1000));
                    }
                    else if (item.Distancia > 200 && item.Distancia <= 500) //Ahora evaluamos rango inferior 200-500
                    {
                        //Actualizamos Aviso
                        UpdateAviso(AvisoItem, item, CalcularConDistanciaAnterior(AvisoItem.Distancia, 200, 500));
                    }
                    else if (item.Distancia > 100 && item.Distancia <= 200) //Ahora evaluamos rango inferior 200-500
                    {
                        //Actualizamos Aviso
                        UpdateAviso(AvisoItem, item, CalcularConDistanciaAnterior(AvisoItem.Distancia, 100, 200));
                    }
                    else if (item.Distancia > 30 && item.Distancia <= 100) //Ahora evaluamos rango inferior 200-500
                    {
                        //Actualizamos Aviso
                        UpdateAviso(AvisoItem, item, CalcularConDistanciaAnterior(AvisoItem.Distancia, 30, 100));
                    }

                }
                else
                {
                    Aviso NuevoItem = NewAviso(item, true);
                    StoreData.Default.PosicionesAvisos.Add(NuevoItem);
                }

            }

            //Eliminamos las entradas anteriores que no estén en la lista de posiciones del GPS

            var resulAvisos = from posAvi in StoreData.Default.PosicionesAvisos where !(from posItem in ListaPosiciones select posItem.Id).Contains(posAvi.Id) select posAvi;

            foreach (Aviso entry in resulAvisos.ToList())
            {
                StoreData.Default.PosicionesAvisos.Remove(entry);
            }

            return StoreData.Default.PosicionesAvisos;
        }

        public static Aviso UpdateAviso(Aviso Item, PosModel Posicion, bool Notificar)
        {
            Item.Id = Posicion.Id;
            Item.Tipo = Posicion.Tipo;
            Item.Distancia = Posicion.Distancia;
            Item.Velocidad = Posicion.Velocidad.ToString();
            Item.Notificar = Notificar;
            Item.Via = Posicion.Via;

            return Item;
        }

        public static Aviso NewAviso(PosModel Posicion, bool Notificar)
        {
            Aviso Item = new Aviso();
            Item.Id = Posicion.Id;
            Item.Tipo = Posicion.Tipo;
            Item.Distancia = Posicion.Distancia;
            Item.Velocidad = Posicion.Velocidad.ToString();
            Item.Notificar = Notificar;
            Item.Via = Posicion.Via;

            return Item;
        }

        private static bool CalcularConDistanciaAnterior(double distanciaAnterior, double desdeRango, double hastaRango)
        {
            //Mismo Rango = 0;
            //Rango Superior = 1;
            //Rango Inferior = -1;            
            int iResultado = 0;
            bool Notificar = false;
            bool EstaEnRangoDistanciaAnterior = (distanciaAnterior > desdeRango && distanciaAnterior <= hastaRango);

            if (!EstaEnRangoDistanciaAnterior)
            {
                if (distanciaAnterior > hastaRango)
                {
                    iResultado = 1;//Se debería notificar, puesto que viene del rango superior
                }
                else
                {
                    iResultado = -1; //La posición se aleja, no deberíamos notificar nada
                }
            }
            else
            {
                iResultado = 0; //Está en el mismo rango, solo se notifica el cambio de rango
            }

            //Evaluamos los resultados
            switch (iResultado)
            {
                case 0:
                case -1:
                    Notificar = false;
                    break;
                case 1:
                    Notificar = true;
                    break;
            }

            return Notificar;
        }

    }
}