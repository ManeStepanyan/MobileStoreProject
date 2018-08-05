using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using MobileStore.Src.Activitys;
using MobileStore.Src.API;
using MobileStore.Src.Dialogs;
using MobileStore.Src.Models;

namespace MobileStore.Src.ListViewAdapters
{
    public partial class ProductsLiostViewAdapter : BaseAdapter<Product>
    {
        protected List<Product> Products;

        public int Layout;
        public Filter Filter { get; private set; }

        protected List<Product> _originalData;
        private Activity Activity;
        protected Context Context;

        public ProductsLiostViewAdapter(Activity context, IEnumerable<Product> products, int layout)
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
                Image.Click += delegate
                {
                    var transaction = this.Activity.FragmentManager.BeginTransaction();
                    var dialog = new ProductDescriptionDialog(this.Context);
                    var pos = position;
                    ActivityCommunication.Product = this.Products[pos];
                    dialog.Show(transaction, "Dialog fragment");
                };
            }

            var ImageView = row.FindViewById<ImageView>(Resource.Id.ProductImageView);
            ImageView.SetImageBitmap(this.Products[position].Image);
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
    }
}