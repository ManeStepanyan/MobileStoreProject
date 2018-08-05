using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileStore.Src.API;
using MobileStore.Src.ListViewAdapters;
using MobileStore.Src.Models;

namespace MobileStore.Src.Dialogs
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
        private readonly ProductsLiostViewAdapter adapter;

        public ProductsFilterDialog(Activity context, ProductsLiostViewAdapter adapter, GridView gridView)
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
            var items = ProductAPIConection.GetBrands();
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
                MinPrice = (this.PriceMinEditText.Text != "") ? (double?)double.Parse(this.PriceMinEditText.Text) : null,
                MaxPrice = (this.PriceMaxEditText.Text != "") ? (double?)double.Parse(this.PriceMaxEditText.Text) : null,
                MinRAM = (this.RAMMinEditText.Text != "") ? (int?)int.Parse(this.RAMMinEditText.Text) : null,
                MaxRAM = (this.RAMMaxEditText.Text != "") ? (int?)int.Parse(this.RAMMaxEditText.Text) : null,
                MinYear = (this.YearMinEditText.Text != "") ? (int?)int.Parse(this.YearMinEditText.Text) : null,
                MaxYear = (this.YearMaxEditText.Text != "") ? (int?)int.Parse(this.YearMaxEditText.Text) : null,
                MinBattery = (this.BatteryMinEditText.Text != "") ? (int?)int.Parse(this.BatteryMinEditText.Text) : null,
                MaxBattery = (this.BatteryMaxEditText.Text != "") ? (int?)int.Parse(this.BatteryMaxEditText.Text) : null,
                MinCamera = (this.CameraMinEditText.Text != "") ? (int?)int.Parse(this.CameraMinEditText.Text) : null,
                MaxCamera = (this.CameraMaxEditText.Text != "") ? (int?)int.Parse(this.CameraMaxEditText.Text) : null,
                MinMemory = (this.MemoryMinEditText.Text != "") ? (int?)int.Parse(this.MemoryMinEditText.Text) : null,
                MaxMemory = (this.MemoryMaxEditText.Text != "") ? (int?)int.Parse(this.MemoryMaxEditText.Text) : null,
                
            };
            var resource = ProductAPIConection.SearchProduct(searchModel);
            this.GridView.Adapter = new ProductsLiostViewAdapter(this.context, resource, Resource.Layout.ProductListViewRow);
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