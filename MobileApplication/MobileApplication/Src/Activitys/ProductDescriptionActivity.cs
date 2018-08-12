using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using MobileApplication.Activitys;
using MobileApplication.Src.API;
using MobileApplication.Src.Cache;
using MobileApplication.Src.Download;
using MobileApplication.Src.Models;
using System.Net;
using System.Threading.Tasks;

namespace MobileApplication.Src.Activitys
{
    [Activity(Label = "ProductDescriptionActivity")]
    public class ProductDescriptionActivity : Activity
    {
        private Product product;
        private ImageView ProductImageView;
        private TextView NameTextView;
        private TextView BarndTextView;
        private TextView BarndVersionView;
        private TextView BarndPriceView;
        private TextView BarndRAMView;
        private TextView BarndYearView;
        private Seller Seller;
        private TextView SellerNameTextView;
        private TextView SellerAddressTextView;
        private TextView SellerCellPhoneTextView;
        private Button AddToCartButton;
        private Button BuyNowButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.product = ActivityCommunication.Product;
            SetContentView(Resource.Layout.ProductDescriptionActivity);

            this.ProductImageView = FindViewById<ImageView>(Resource.Id.ProductImageView);
            new Task(() => {
                var ulr = this.product.Image;
                if (ImageCache.Cache.ContainsKey(ulr))
                {
                    this.ProductImageView.SetImageBitmap(ImageCache.Cache[ulr]);
                }
                else
                {
                    var image = ImageDownload.GetImageBitmapFromUrl(ulr);
                    this.ProductImageView.SetImageBitmap(image);
                    ImageCache.Cache.Add(ulr, image);
                }
            }).Start();
            this.NameTextView = FindViewById<TextView>(Resource.Id.ProductNameTextView);
            this.NameTextView.Text = this.product.Name;
            this.BarndTextView = FindViewById<TextView>(Resource.Id.ProductBrandTextView);
            this.BarndTextView.Text = this.product.Brand;
            this.BarndVersionView = FindViewById<TextView>(Resource.Id.ProductVersionTextView);
            this.BarndVersionView.Text = this.product.Version;
            this.BarndPriceView = FindViewById<TextView>(Resource.Id.ProductPriceTextView);
            this.BarndPriceView.Text = this.product.Price.ToString();
            this.BarndRAMView = FindViewById<TextView>(Resource.Id.ProductRAMTextView);
            this.BarndRAMView.Text = this.product.RAM.ToString();
            this.BarndYearView = FindViewById<TextView>(Resource.Id.ProductYearTextView);
            this.BarndYearView.Text = this.product.Year.ToString();

            this.Seller = UserAPIController.GetSellerById(CatalogAPIController.GetSellerIdByProductId(this.product.Id).Result).Result;

            this.SellerNameTextView = FindViewById<TextView>(Resource.Id.SellerNameTextView);
            this.SellerNameTextView.Text = this.Seller.Name;
            this.SellerAddressTextView = FindViewById<TextView>(Resource.Id.SellerAddressTextView);
            this.SellerAddressTextView.Text = this.Seller.Address;
            this.SellerCellPhoneTextView = FindViewById<TextView>(Resource.Id.SellerCellPhoneTextView);
            this.SellerCellPhoneTextView.Text = this.Seller.CellPhone;

            this.AddToCartButton = FindViewById<Button>(Resource.Id.AddToCartButton);
            this.AddToCartButton.Click += AddToCartButton_Click;

            this.BuyNowButton = FindViewById<Button>(Resource.Id.BuyNowButton);
            this.BuyNowButton.Click += BuyNowButton_Click;

            FindViewById<LinearLayout>(Resource.Id.SellerDescriptionLinearLayout).Click += SellerDescriptionLinerLayout_Click;
        }

        private void SellerDescriptionLinerLayout_Click(object sender, System.EventArgs e)
        {
            var newActivity = new Intent(this, typeof(SellerDescriptionActivity));
            ActivityCommunication.Seller = UserAPIController.GetSellerById(CatalogAPIController.GetSellerIdByProductId(this.product.Id).Result).Result;
            StartActivity(newActivity);
        }

        private void BuyNowButton_Click(object sender, System.EventArgs e)
        {
            var newActivity = new Intent(this,
                (UserAPIController.SessionActivity()) ? typeof(BuyActivity) : typeof(SignInActivity));
            StartActivity(newActivity);
        }

        private void AddToCartButton_Click(object sender, System.EventArgs e)
        {
            if (UserAPIController.SessionActivity())
            {
                var messige = (OrdersAndShopCartAPIController.AddProduct(this.product.Id).Result) ? "Add to cart." : "Has already.";
                Toast.MakeText(this, messige, ToastLength.Long).Show();
            }
            else
            {
                var newActivity = new Intent(this, typeof(SignInActivity));
                StartActivity(newActivity);
            }
        }
    }
}