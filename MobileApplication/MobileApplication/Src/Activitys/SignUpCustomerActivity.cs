using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using MobileApplication.Src.API;

namespace MobileApplication.Activitys
{
    [Activity(Label = "Mobile Store")]
    class SignUpCustomerActivity : Activity
    {
        private TextView NameTitleTextView;
        private TextView NameTextView;
        private TextView SureNameTextView;
        private TextView LoginTextView;
        private TextView EmailTextView;
        private TextView PasswordTextView;
        private TextView PasswordTextView1;
        private Button SignUpButton;
        private TextView PasswordErrorTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUpCustomerActivity);

            this.NameTitleTextView = FindViewById<TextView>(Resource.Id.signup_client);

            this.NameTextView = FindViewById<TextView>(Resource.Id.name_client);
            this.SureNameTextView = FindViewById<TextView>(Resource.Id.sname_client);
            this.LoginTextView = FindViewById<TextView>(Resource.Id.login_client);
            this.EmailTextView = FindViewById<TextView>(Resource.Id.email_client);
            this.PasswordTextView = FindViewById<TextView>(Resource.Id.password_client);
            this.PasswordTextView1 = FindViewById<TextView>(Resource.Id.password_client1);
            this.PasswordTextView.TextChanged += PasswordTextView_TextChanged;
            this.PasswordTextView1.TextChanged += PasswordTextView_TextChanged;
            this.SignUpButton = FindViewById<Button>(Resource.Id.btn_signup);
            this.SignUpButton.Click += SignUpButton_Click;
            this.PasswordErrorTextView = FindViewById<TextView>(Resource.Id.PasswordErrorTextView);
        }

        private void PasswordTextView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if(this.PasswordTextView.Text != this.PasswordTextView1.Text)
            {
                this.PasswordErrorTextView.Text = "Passwords do not match.";
            }
            else
            {
                this.PasswordErrorTextView.Text = "";
            }
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            if (this.PasswordTextView.Text != this.PasswordTextView1.Text)
            {
                View view = (View)sender;
                Snackbar.Make(view, "Passwords do not match.", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                return;
            }

            if (
                this.NameTextView.Text == "" ||
                this.SureNameTextView.Text == "" ||
                this.LoginTextView.Text == "" ||
                this.EmailTextView.Text == "" ||
                this.PasswordTextView.Text == "")
            {
                this.NameTitleTextView.Text = "Name\nPlease enter all the fields.";
            }

            if (!UserAPIConection.RegisterCustomer(
                this.NameTextView.Text,
                this.SureNameTextView.Text,
                this.LoginTextView.Text,
                this.EmailTextView.Text,
                this.PasswordTextView.Text))
            {
                this.NameTitleTextView.Text = "This login already exists.";
            }

            //var nextActivity = new Intent(this, typeof(HomeActivity));
            //StartActivity(nextActivity);
            StartActivity(typeof(MainActivity));
        }
    }
}