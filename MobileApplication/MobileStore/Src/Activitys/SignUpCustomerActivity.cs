using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MobileStore.Src.API;

namespace MobileStore.Activitys
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
        private Button SignUpButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SingUpCustomerActivity);

            this.NameTitleTextView = FindViewById<TextView>(Resource.Id.signup_client);

            this.NameTextView = FindViewById<TextView>(Resource.Id.name_client);
            this.SureNameTextView = FindViewById<TextView>(Resource.Id.sname_client);
            this.LoginTextView = FindViewById<TextView>(Resource.Id.login_client);
            this.EmailTextView = FindViewById<TextView>(Resource.Id.email_client);
            this.PasswordTextView = FindViewById<TextView>(Resource.Id.password_client);

            this.SignUpButton = FindViewById<Button>(Resource.Id.btn_signup);
            this.SignUpButton.Click += SignUpButton_Click;
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            //if (
            //    this.NameTextView.Text == "" ||
            //    this.SureNameTextView.Text  == "" ||
            //    this.LoginTextView.Text == "" ||
            //    this.EmailTextView.Text == "" ||
            //    this.PasswordTextView.Text == "")
            //{
            //    this.NameTitleTextView.Text = "Name\nPlease enter all the fields.";
            //}

            if (!UserAPIConection.RegisterCustomer(
                this.NameTextView.Text,
                this.SureNameTextView.Text,
                this.LoginTextView.Text,
                this.EmailTextView.Text,
                this.PasswordTextView.Text))
            {
                this.NameTitleTextView.Text = "This login already exists.";
            }

            var nextActivity = new Intent(this, typeof(HomeActivity));
            StartActivity(nextActivity);
        }
    }
}