using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
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
    public class MainActivity : AppCompatActivity
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

            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var GoTocartPageFloatingActionButton = FindViewById<FloatingActionButton>(Resource.Id.GoToCartPageButton);
            GoTocartPageFloatingActionButton.Click += GoToCartPage_Click;


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
                                (UserAPIConection.SessionActivity()) ? typeof(CartActivity) : typeof(SignInActivity));
                        StartActivity(newActivity);
                        break;
                    }
                case Resource.Id.action_account:
                    {
                        var newActivity = new Intent(this,
                                (UserAPIConection.SessionActivity()) ? typeof(MyAccountActivity) : typeof(SignInActivity));
                        StartActivity(newActivity);
                        break;
                    }
                default:
                    return base.OnOptionsItemSelected(item);
            }

            return true;
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            //View view = (View)sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
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

