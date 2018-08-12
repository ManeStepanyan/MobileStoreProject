using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileApplication.Src.API;
using MobileApplication.Src.ListViewAdapters;
using MobileApplication.Src.Models;

namespace MobileApplication.Src.Dialogs
{
    public class ProductsFilterDialog : DialogFragment
    {
        private EditText PriceMinEditText;
        private EditText PriceMaxEditText;
        private EditText MemoryMinEditText;
        private EditText MemoryMaxEditText;
        private EditText RAMMinEditText;
        private EditText RAMMaxEditText;
        private EditText YearMinEditText;
        private EditText YearMaxEditText;
        private EditText BatteryMinEditText;
        private EditText BatteryMaxEditText;
        private EditText CameraMinEditText;
        private EditText CameraMaxEditText;
        private AutoCompleteTextView BrandAutoCompleteTextView;
        private Button DoneButton;

        public GridView GridView;

        private readonly Activity context;
        private readonly ProductsAdapter adapter;

        public ProductsFilterDialog(Activity context, ProductsAdapter adapter, GridView gridView)
        {
            this.GridView = gridView;
            this.context = context;
            this.adapter = adapter;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.ProductFilterDialog, container, false);
            this.PriceMinEditText = view.FindViewById<EditText>(Resource.Id.PriceMinEditText);
            this.PriceMaxEditText = view.FindViewById<EditText>(Resource.Id.PriceMaxEditText);
            this.MemoryMinEditText = view.FindViewById<EditText>(Resource.Id.MemoryMinEditText);
            this.MemoryMaxEditText = view.FindViewById<EditText>(Resource.Id.MemoryMaxEditText);
            this.RAMMinEditText = view.FindViewById<EditText>(Resource.Id.RAMMinEditText);
            this.RAMMaxEditText = view.FindViewById<EditText>(Resource.Id.RAMMaxEditText);
            this.YearMinEditText = view.FindViewById<EditText>(Resource.Id.YearMinEditText);
            this.YearMaxEditText = view.FindViewById<EditText>(Resource.Id.YearMaxEditText);
            this.BatteryMinEditText = view.FindViewById<EditText>(Resource.Id.BatteryMinEditText);
            this.BatteryMaxEditText = view.FindViewById<EditText>(Resource.Id.BatteryMaxEditText);
            this.CameraMinEditText = view.FindViewById<EditText>(Resource.Id.CameraMinEditText);
            this.CameraMaxEditText = view.FindViewById<EditText>(Resource.Id.CameraMaxEditText);

            this.BrandAutoCompleteTextView = view.FindViewById<AutoCompleteTextView>(Resource.Id.BrandAutoCompleteTextView);
            var items = ProductAPIController.GetBrands();
            var adapter = new ArrayAdapter(this.Context, Android.Resource.Layout.SimpleDropDownItem1Line, items);
            this.BrandAutoCompleteTextView.Adapter = adapter;

            this.DoneButton = view.FindViewById<Button>(Resource.Id.DoneButton);
            this.DoneButton.Click += DoneButton_Click;
            return view;
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            var searchModel = new SearchProductModel()
            {
                Brand = this.BrandAutoCompleteTextView.Text,
                Price = (this.PriceMinEditText.Text != "") ? (decimal?)decimal.Parse(this.PriceMinEditText.Text) : null,
                PriceTo = (this.PriceMaxEditText.Text != "") ? (decimal?)decimal.Parse(this.PriceMaxEditText.Text) : null,
                RAM = (this.RAMMinEditText.Text != "") ? (int?)int.Parse(this.RAMMinEditText.Text) : null,
                RAMTo = (this.RAMMaxEditText.Text != "") ? (int?)int.Parse(this.RAMMaxEditText.Text) : null,
                Year = (this.YearMinEditText.Text != "") ? (int?)int.Parse(this.YearMinEditText.Text) : null,
                YearTo = (this.YearMaxEditText.Text != "") ? (int?)int.Parse(this.YearMaxEditText.Text) : null,
                Battery = (this.BatteryMinEditText.Text != "") ? (int?)int.Parse(this.BatteryMinEditText.Text) : null,
                BatteryTo = (this.BatteryMaxEditText.Text != "") ? (int?)int.Parse(this.BatteryMaxEditText.Text) : null,
                Camera = (this.CameraMinEditText.Text != "") ? (int?)int.Parse(this.CameraMinEditText.Text) : null,
                CameraTo = (this.CameraMaxEditText.Text != "") ? (int?)int.Parse(this.CameraMaxEditText.Text) : null,
                Memory = (this.MemoryMinEditText.Text != "") ? (int?)int.Parse(this.MemoryMinEditText.Text) : null,
                MemoryTo = (this.MemoryMaxEditText.Text != "") ? (int?)int.Parse(this.MemoryMaxEditText.Text) : null,
                
            };
            try
            {
                var resource = ProductAPIController.SearchProduct(searchModel).Result;
                this.GridView.Adapter = new ProductsAdapter(this.context, resource, Resource.Layout.ProductAdapterItem);
            }
            catch (Exception)
            {
                
                throw;
            }
            
            this.Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnimation;
        }
    }
}