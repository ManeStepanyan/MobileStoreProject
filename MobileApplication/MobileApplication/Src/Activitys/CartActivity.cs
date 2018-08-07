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
using MobileApplication.Src.API;
using MobileApplication.Src.ListViewAdapters;
using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;
namespace MobileApplication.Src.Activitys
{
    [Activity(Label = "CartActivity", MainLauncher = false)]
    public class CartActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CartActivity);

            var products = OrdersAndShopCartAPIConection.GetAllProducts().Select(a => ProductAPIConection.GetProductById(a));
            var adapter = new CartListViewAdapter(this, products, Resource.Layout.CartListViewRow);

            var ListView = FindViewById<ListView>(Resource.Id.CartListView);
            ListView.Adapter = adapter;

            var BuyButton = FindViewById<FloatingActionButton>(Resource.Id.BuyButton);
            BuyButton.Click += BuyButton_Click;
        }

        private void BuyButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(BuyActivity));
            StartActivity(intent);
        }
    }
}