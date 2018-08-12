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
using MobileApplication.Activitys;
using MobileApplication.Src.API;
using MobileApplication.Src.ListViewAdapters;
using MobileApplication.Src.Models;

namespace MobileApplication.Src.Activitys
{
    [Activity(Label = "SellerDescriptionActivity", MainLauncher = false)]
    public class SellerDescriptionActivity : Activity
    {
        private ProductsAdapter Adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SellerDescriptionActivity);
            var seller = ActivityCommunication.Seller;
            FindViewById<TextView>(Resource.Id.SellerNameTextView).Text = seller.Name;
            FindViewById<RatingBar>(Resource.Id.SellerRatingBar).Rating = (float)seller.Rating;
            FindViewById<TextView>(Resource.Id.SellerAddressTextView).Text = seller.Address;
            FindViewById<TextView>(Resource.Id.SellerCellPhoneTextView).Text = seller.CellPhone;
            FindViewById<TextView>(Resource.Id.SellerEmailTextView).Text = seller.Email;
            var products = CatalogAPIController.GetProductsBySellerId(seller.Id);
            this.Adapter = new ProductsAdapter(this, products, Resource.Layout.ProductAdapterItem);
            var ProductsGridView = FindViewById<GridView>(Resource.Id.ProductListView);
            ProductsGridView.Adapter = Adapter;
            ProductsGridView.ItemClick += ProductsGridView_ItemClick;
        }

        private void ProductsGridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(ProductDescriptionActivity));
            ActivityCommunication.Product = this.Adapter[e.Position];
            StartActivity(intent);
        }
    }
}