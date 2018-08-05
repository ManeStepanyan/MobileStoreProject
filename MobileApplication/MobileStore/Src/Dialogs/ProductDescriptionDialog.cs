﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using MobileStore.Activitys;
using MobileStore.Src.Activitys;
using MobileStore.Src.API;
using MobileStore.Src.Models;

namespace MobileStore.Src.Dialogs
{
    public class ProductDescriptionDialog : DialogFragment
    {
        private readonly Context context;
        private Product product;

        public ProductDescriptionDialog(Context context) => this.context = context;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            this.product = ActivityCommunication.Product;
            var view = inflater.Inflate(Resource.Layout.ProductDescriptionDialog, container, false);
            var ImageView = view.FindViewById<ImageView>(Resource.Id.ProductImageView);
            ImageView.SetImageBitmap(this.product.Image);
            var NameTextView = view.FindViewById<TextView>(Resource.Id.ProductNameTextView);
            NameTextView.Text = this.product.Name;
            var BrandTextView = view.FindViewById<TextView>(Resource.Id.ProductBrandTextView);
            BrandTextView.Text = this.product.Brand;
            var PriceTextView = view.FindViewById<TextView>(Resource.Id.ProductPriceTextView);
            PriceTextView.Text = this.product.Price.ToString();

            var AddToCartButton = view.FindViewById<Button>(Resource.Id.AddToCartButton);
            AddToCartButton.Click += AddToCartButton_Click;

            var BuyNowButton = view.FindViewById<Button>(Resource.Id.BuyNowButton);
            BuyNowButton.Click += BuyNowButton_Click;
            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.DialogAnimation;
        }

        private void BuyNowButton_Click(object sender, System.EventArgs e)
        {
            var newActivity = new Intent(this.context, 
                (UserAPIConection.SessionActivity()) ? typeof(BuyActivity) : typeof(SignInActivity));
            StartActivity(newActivity);
        }

        private void AddToCartButton_Click(object sender, System.EventArgs e)
        {
            if (UserAPIConection.SessionActivity())
            {
                var messige = (OrdersAndShopCartAPIConection.AddProduct(this.product.Id)) ? "Add to cart." : "Has already.";
                Toast.MakeText(this.context, messige, ToastLength.Long).Show();
            }
            else
            {
                var newActivity = new Intent(this.context, typeof(SignInActivity));
                StartActivity(newActivity);
            }
        }
    }
}