using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MobileApplication;
using MobileApplication.Src.Activitys;
using MobileApplication.Src.API;
using MobileApplication.Src.Dialogs;
using MobileApplication.Src.Models;

namespace MobileApplication.Src.ListViewAdapters
{
    public partial class ProductsAdapter : BaseAdapter<Product>
    {
        protected List<Product> Products;
        protected static Dictionary<string, Bitmap> ImageCache = new Dictionary<string, Bitmap>();
        public int Layout;
        public Filter Filter { get; private set; }

        protected List<Product> _originalData;
        private Activity Activity;
        protected Context Context;

        public ProductsAdapter(Activity context, IEnumerable<Product> products, int layout)
        {
            this.Activity = context;
            this.Context = context;
            this.Products = products.ToList();
            this.Layout = layout;
            this.Filter = new ProductFilter(this);
        }

        public void SetNewItems(IEnumerable<Product> products)
        {
            this.Products = products.ToList();
            this._originalData = products.ToList();
        }

        public override Product this[int position] => this.Products[position];

        public override int Count => this.Products.Count;

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(this.Context).Inflate(this.Layout, null, false);
                var Image = row.FindViewById<ImageView>(Resource.Id.DetalsImageView);
                var dialogGenericTask = new Task<ProductDescriptionDialog>(() => new ProductDescriptionDialog(this.Context));
                dialogGenericTask.Start();
                var transactionGenericTask = new Task<FragmentTransaction>(() => this.Activity.FragmentManager.BeginTransaction());
                transactionGenericTask.Start();
                Image.Click += delegate
                {
                    //var transaction = this.Activity.FragmentManager.BeginTransaction();
                    //var dialog = new ProductDescriptionDialog(this.Context);
                    var pos = position;
                    ActivityCommunication.Product = this.Products[pos];
                    dialogGenericTask.Result.Show(transactionGenericTask.Result, "Dialog fragment");
                };
            }

            var ImageView = row.FindViewById<ImageView>(Resource.Id.ProductImageView);
            new Task(() => {
                var ulr = this.Products[position].Image;
                if (ImageCache.ContainsKey(ulr))
                {
                    ImageView.SetImageBitmap(ImageCache[ulr]);
                }
                else
                {
                    var image = GetImageBitmapFromUrl(ulr);
                    ImageView.SetImageBitmap(image);
                    ImageCache.Add(ulr, image);
                } 
                }).Start();
            var NameTextView = row.FindViewById<TextView>(Resource.Id.ProductNameTextView);
            NameTextView.Text = this.Products[position].Name;




            //var PriceTextView = row.FindViewById<TextView>(Resource.Id.ProductPriceTextView);
            //PriceTextView.Text = $"{this.Products[position].Price.ToString()} $";


            return row;
        }

        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
        }

        private static Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}