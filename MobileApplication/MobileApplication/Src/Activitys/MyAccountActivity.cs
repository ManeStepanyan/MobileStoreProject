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
        private EditText NameEditText;
        private EditText SureNameEditText;
        private EditText EmailEditText;
        private EditText PasswordEditText;
        private EditText PasswordEditText2;
        private Button SaveUpButton;
        private TextView PasswordErrorTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.MyAccountActivity);

            var user = UserAPIConection.User;
            this.NameEditText = FindViewById<EditText>(Resource.Id.name_client);
            this.NameEditText.Hint = user.Name;
            this.SureNameEditText = FindViewById<EditText>(Resource.Id.sname_client);
            this.SureNameEditText.Hint = user.Surname;
            this.EmailEditText = FindViewById<EditText>(Resource.Id.email_client);
            this.EmailEditText.Hint = user.Email;
            this.PasswordEditText = FindViewById<EditText>(Resource.Id.password_client);
            this.PasswordEditText2 = FindViewById<EditText>(Resource.Id.password_client2);
            this.PasswordEditText.TextChanged += PasswordTextView_TextChanged;
            this.PasswordEditText2.TextChanged += PasswordTextView_TextChanged;
            this.SaveUpButton = FindViewById<Button>(Resource.Id.SaveButton);
            this.SaveUpButton.Click += SaveUpButton_Click;
            this.PasswordErrorTextView = FindViewById<TextView>(Resource.Id.PasswordErrorTextView);
        }

        private void PasswordTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (this.PasswordEditText.Text != this.PasswordEditText2.Text)
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
            if (this.PasswordEditText.Text != this.PasswordEditText2.Text)
            {
                View view = (View)sender;
                Snackbar.Make(view, "Passwords do not match.", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                return;
            }

            var user = UserAPIConection.User;
            var newUser = new UserModel(
                user.Name, this.SureNameEditText.Text,
                null,
                null,
                user.Login,
                this.PasswordEditText.Text,
                null,
                null);

            UserAPIConection.UpdateCustomer(newUser);
            StartActivity(typeof(HomeActivity));
        }
    }
}