﻿using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using System.Collections.Generic;
using MobileApplication.Src.ListViewAdapters;
using System.Threading.Tasks;
using Android.Content;
using MobileApplication.Src.Models;
using MobileApplication.Src.Dialogs;
using MobileApplication.Src.API;
using MobileApplication.Src.Activitys;
using MobileApplication.Activitys;
using SearchView = Android.Support.V7.Widget.SearchView;
using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;


namespace MobileApplication
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class HomeActivity : AppCompatActivity
    {
        private SearchView SearchView;
        private List<Product> Products;
        private ProductsAdapter Adapter;
        private GridView ProductsGridView;
        private ImageView FilterImageView;
        private Spinner SortBySpiner;


        private Task<Intent> ProductDescriptionActivityGenericTask;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var GoTocartPageFloatingActionButton = FindViewById<FloatingActionButton>(Resource.Id.GoToCartPageButton);
            GoTocartPageFloatingActionButton.Click += GoToCartPage_Click;


            this.SearchView = FindViewById<SearchView>(Resource.Id.searchView);
            this.SearchView.QueryTextChange += SearchView_QueryTextChange;
            this.Products = ProductAPIController.GetProducts().Result;
            this.Adapter = new ProductsAdapter(this, this.Products, Resource.Layout.ProductAdapterItem);
            this.ProductsGridView = FindViewById<GridView>(Resource.Id.ProductListView);
            this.ProductsGridView.ItemClick += ProductsGridView_ItemClick;
            this.FilterImageView = FindViewById<ImageView>(Resource.Id.FilterImageView);
            this.FilterImageView.Click += FilterImageView_Click;


            this.SortBySpiner = FindViewById<Spinner>(Resource.Id.SortBySpiner);
            this.SortBySpiner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleExpandableListItem1, SearchByItems);
            this.SortBySpiner.ItemSelected += SortBySpiner_ItemSelected;



            this.ProductDescriptionActivityGenericTask = new Task<Intent>(() => new Intent(this, typeof(ProductDescriptionActivity)));
            this.ProductDescriptionActivityGenericTask.Start();
            this.CartActivityGenericTask = new Task<Intent>(() => new Intent(this, typeof(CartActivity)));
            this.CartActivityGenericTask.Start();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            switch (item.ItemId)
            {
                case Resource.Id.action_cart:
                    {
                        var newActivity = new Intent(this,
                                (UserAPIController.SessionActivity()) ? typeof(CartActivity) : typeof(SignInActivity));
                        StartActivity(newActivity);
                        break;
                    }
                case Resource.Id.action_account:
                    {
                        var newActivity = new Intent(this,
                                (UserAPIController.SessionActivity()) ? typeof(MyAccountActivity) : typeof(SignInActivity));
                        StartActivity(newActivity);
                        break;
                    }
                default:
                    return base.OnOptionsItemSelected(item);
            }

            return true;
        }

        private void SortBySpiner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Console.WriteLine(e.Position);
            Comparison<Product> compare = null;

            switch (e.Position)
            {
                case 0:
                    compare = (a, b) => a.Name.CompareTo(b.Name);
                    break;
                case 1:
                    compare = (a, b) => ((decimal)a.Price).CompareTo((decimal)b.Price);
                    break;
                case 2:
                    compare = (a, b) => ((double)a.Memory).CompareTo((double)b.Memory);
                    break;
                case 3:
                    compare = (a, b) => ((double)a.RAM).CompareTo((double)b.RAM);
                    break;
                default:
                    throw new Exception("Inadmissible situation.");
            }

            this.Products.Sort(compare);
            this.Adapter = new ProductsAdapter(this, this.Products, Resource.Layout.ProductAdapterItem);
            this.ProductsGridView.Adapter = this.Adapter;

            Console.WriteLine("==============");
        }

        private void FilterImageView_Click(object sender, System.EventArgs e)
        {
            var transaction = FragmentManager.BeginTransaction();
            var dialog = new ProductsFilterDialog(this, Adapter, this.ProductsGridView);
            dialog.Show(transaction, "Dialog fragment");
        }

        private void GoToCartPage_Click(object sender, System.EventArgs e)
        {
            var newActivity = (UserAPIController.SessionActivity()) ? this.CartActivityGenericTask.Result :
                new Intent(this, typeof(SignInActivity));
            StartActivity(newActivity);
        }

        private void ProductsGridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ActivityCommunication.Product = this.Adapter[e.Position];
            StartActivity(this.ProductDescriptionActivityGenericTask.Result);
        }

        private void SearchView_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            this.Adapter.Filter.InvokeFilter(e.NewText);
        }


        public static readonly string[] SearchByItems = new string[]
        {
            "Name",
            "Price",
            "Memory",
            "RAM"
        };

        public Task<Intent> CartActivityGenericTask { get; private set; }
    }
}

