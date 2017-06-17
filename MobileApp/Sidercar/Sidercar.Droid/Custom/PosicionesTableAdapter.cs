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

namespace Sidercar.Droid.Custom
{
    public class PosicionesTableAdapter : BaseAdapter<PosModel>
    {
        List<PosModel> items;
        Activity context;

        public PosicionesTableAdapter(Activity context, List<PosModel> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override PosModel this[int position]
        {
            get { return items.ElementAt(position); }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null)
            { // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            }
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items.ElementAt(position).Via;
            //view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items.ElementAt(position).Latitud + ","+ items.ElementAt(position).Longitud;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = string.Format("Distancia {0} metros", items.ElementAt(position).Distancia.ToString());

            //var imageBitmap = gestion.GetImageBitmapFromUrl(items.ElementAt(position).avatar);
            //view.FindViewById<ImageView>(Resource.Id.Image).SetImageBitmap(imageBitmap);

            //var ImageView = view.FindViewById<ImageView>(Resource.Id.Image);
            //Picasso.With(context).Load(items.ElementAt(position).avatar).Into(ImageView);

            return view;
        }
    }
}