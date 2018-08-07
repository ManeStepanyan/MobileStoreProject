using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MobileApplication.Src.API;
using MobileApplication.Src.Models;

namespace MobileApplication.Src.Activitys
{
    [Activity(Label = "MyAccountActivity")]
    public class MyAccountActivity : Activity
    {
        private TextView NameTextView;
        private TextView SureNameTextView;
        private TextView EmailTextView;
        private TextView PasswordTextView;
        private TextView PasswordTextView2;
        private Button SaveUpButton;
        private TextView PasswordErrorTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyAccountActivity);

            var user = UserAPIConection.User;
            this.NameTextView = FindViewById<TextView>(Resource.Id.name_client);
            this.NameTextView.Hint = user.Name;
            this.SureNameTextView = FindViewById<TextView>(Resource.Id.sname_client);
            this.SureNameTextView.Hint = user.Surname;
            this.EmailTextView = FindViewById<TextView>(Resource.Id.email_client);
            this.EmailTextView.Hint = user.Email;
            this.PasswordTextView = FindViewById<TextView>(Resource.Id.password_client);
            this.PasswordTextView2 = FindViewById<TextView>(Resource.Id.password_client2);
            this.PasswordTextView.TextChanged += PasswordTextView_TextChanged;
            this.PasswordTextView2.TextChanged += PasswordTextView_TextChanged;
            this.SaveUpButton = FindViewById<Button>(Resource.Id.SaveButton);
            this.SaveUpButton.Click += SaveUpButton_Click;
            this.PasswordErrorTextView = FindViewById<TextView>(Resource.Id.PasswordErrorTextView);
        }

        private void PasswordTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (this.PasswordTextView.Text != this.PasswordTextView2.Text)
            {
                this.PasswordErrorTextView.Text = "Passwords do not match.";
            }
            else
            {
                this.PasswordErrorTextView.Text = "";
            }
        }

        private void SaveUpButton_Click(object sender, EventArgs e)
        {
            if (this.PasswordTextView.Text != this.PasswordTextView2.Text)
            {
                View view = (View)sender;
                Snackbar.Make(view, "Passwords do not match.", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                return;
            }

            var user = UserAPIConection.User;
            var newUser = new UserModel(
                user.Name, this.SureNameTextView.Text,
                null,
                null,
                user.Login,
                this.PasswordTextView.Text,
                null,
                null);

            UserAPIConection.UpdateCustomer(newUser);
            StartActivity(typeof(MainActivity));
        }
    }
}