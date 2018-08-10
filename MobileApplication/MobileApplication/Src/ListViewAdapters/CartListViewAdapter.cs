using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using MobileApplication.Src.Models;

namespace MobileApplication.Src.ListViewAdapters
{
    public class CartListViewAdapter : ProductsAdapter
    {
        private Dictionary<int, int> ProductCount;

        public CartListViewAdapter(Activity context, IEnumerable<Product> products, int layout) : base(context, products, layout)
        {
            this.ProductCount = new Dictionary<int, int>();
            for (var i = 0; i < products.Count(); ++i)
            {
                this.ProductCount.Add(i, 0);
            }
        }


        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;
            if (row == null)
            {
                row = LayoutInflater.From(this.Context).Inflate(this.Layout, null, false);
            }
            var ImageView = row.FindViewById<ImageView>(Resource.Id.ProductImageView);
            //ImageView.SetImageBitmap(this.Products[position].Image);
            var NameTextView = row.FindViewById<TextView>(Resource.Id.ProductNameTextView);
            NameTextView.Text = this.Products[position].Name;
            var PriceTextView = row.FindViewById<TextView>(Resource.Id.ProductPriceTextView);
            PriceTextView.Text = $"{this.Products[position].Price.ToString()} $";
            var BrandTextView = row.FindViewById<TextView>(Resource.Id.ProductBrandTextView);
            BrandTextView.Text = this.Products[position].Brand;


            if (convertView == null)
            {
                var CountTextView = row.FindViewById<TextView>(Resource.Id.CountTextView);
                var PlusImageView = row.FindViewById<ImageView>(Resource.Id.PlusImageView);
                var MinusImageView = row.FindViewById<ImageView>(Resource.Id.MinusImageView);
                PlusImageView.Click += delegate
                {
                    Console.WriteLine("==============================");

                    var pos = position;
                    if (this.Products[pos].Quantity > this.ProductCount[pos])
                    {
                        this.ProductCount[pos]++;
                    }
                    else
                    {
                        Toast.MakeText(this.Context, "The maximum number.", ToastLength.Short).Show();
                    }
                    var TempCountTextView = CountTextView;
                    TempCountTextView.Text = this.ProductCount[pos].ToString();
                };

                MinusImageView.Click += delegate
                {
                    var pos = position;
                    if (this.ProductCount[pos] > 0)
                    {
                        this.ProductCount[pos]--;
                    }
                    var TempCountTextView = CountTextView;
                    TempCountTextView.Text = this.ProductCount[position].ToString();
                };
            }
            return row;
        }


    }
}