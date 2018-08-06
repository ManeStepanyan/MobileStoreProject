using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using MobileStore.Src.ListViewAdapters;
using MobileStore.Src.API;
using MobileStore.Src.Activitys;
using Android.Content;
using SearchView = Android.Support.V7.Widget.SearchView;
using FloatingActionButton = Android.Support.Design.Widget.FloatingActionButton;
using MobileStore.Src.Dialogs;
using System;
using MobileStore.Src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileStore.Activitys
{
    [Activity(Label = "Mobile Store", MainLauncher = false)]
    class HomeActivity : Activity
    {
        private SearchView SearchView;
        private List<Product> Products;
        private ProductsLiostViewAdapter Adapter;
        private GridView ProductsGridView;
        private ImageView GoToCartPageImageView;
        private ImageView FilterImageView;
        private Spinner SortBySpiner;


        private Task<Intent> ProductDescriptionActivityGenericTask;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.HomeActivity);

            this.SearchView = FindViewById<SearchView>(Resource.Id.searchView);
            this.SearchView.QueryTextChange += SearchView_QueryTextChange;
            this.Products = ProductAPIConection.GetProducts();
            this.Adapter = new ProductsLiostViewAdapter(this, this.Products, Resource.Layout.ProductListViewRow);
            this.ProductsGridView = FindViewById<GridView>(Resource.Id.ProductListView);
            //this.ProductsGridView.Adapter = this.Adapter;
            this.ProductsGridView.ItemClick += ProductsGridView_ItemClick;
            this.GoToCartPageImageView = FindViewById<ImageView>(Resource.Id.GoToCartPageImageView);
            this.GoToCartPageImageView.Click += GoToCartPage_Click;
            this.FilterImageView = FindViewById<ImageView>(Resource.Id.FilterImageView);
            this.FilterImageView.Click += FilterImageView_Click;


            this.SortBySpiner = FindViewById<Spinner>(Resource.Id.SortBySpiner);
            this.SortBySpiner.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleExpandableListItem1, SearchByItems);
            this.SortBySpiner.ItemSelected += SortBySpiner_ItemSelected;



            this.ProductDescriptionActivityGenericTask = new Task<Intent>(() => new Intent(this, typeof(ProductDescriptionActivity)));
            this.ProductDescriptionActivityGenericTask.Start();
            
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
                    compare = (a, b) => a.Price.CompareTo(b.Price);
                    break;
                case 2:
                    compare = (a, b) => a.Memory.CompareTo(b.Memory);
                    break;
                case 3:
                    compare = (a, b) => a.RAM.CompareTo(b.RAM);
                    break;
                default:
                    throw new Exception("Inadmissible situation.");
            }

            this.Products.Sort(compare);
            this.Adapter = new ProductsLiostViewAdapter(this, this.Products, Resource.Layout.ProductListViewRow);
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
            var newActivity = new Intent(this,
                (UserAPIConection.SessionActivity()) ? typeof(CartActivity) : typeof(SignInActivity));
            StartActivity(newActivity);
        }

        private void ProductsGridView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(ProductDescriptionActivity));
            ActivityCommunication.Product = this.Adapter[e.Position];
            //StartActivity(intent);
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

    }
}