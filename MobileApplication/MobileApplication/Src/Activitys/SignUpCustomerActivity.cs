using System;
using Android.App;
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
        private TextView ErrorTextView;
        private EditText NameEditText;
        private EditText SurNameEditText;
        private EditText LoginEditText;
        private EditText EmailEditText;
        private EditText PasswordEditText;
        private EditText PasswordEditText1;
        private Button SignUpButton;
        private TextView PasswordErrorTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUpCustomerActivity);

            this.ErrorTextView = FindViewById<TextView>(Resource.Id.signup_client);

            this.NameEditText = FindViewById<EditText>(Resource.Id.name_client);
            this.SurNameEditText = FindViewById<EditText>(Resource.Id.sname_client);
            this.LoginEditText = FindViewById<EditText>(Resource.Id.login_client);
            this.EmailEditText = FindViewById<EditText>(Resource.Id.email_client);
            this.PasswordEditText = FindViewById<EditText>(Resource.Id.password_client);
            this.PasswordEditText1 = FindViewById<EditText>(Resource.Id.password_client1);
            this.PasswordEditText.TextChanged += PasswordEditText_TextChanged;
            this.PasswordEditText1.TextChanged += PasswordEditText_TextChanged;
            this.SignUpButton = FindViewById<Button>(Resource.Id.btn_signup);
            this.SignUpButton.Click += SignUpButton_Click;
            this.PasswordErrorTextView = FindViewById<TextView>(Resource.Id.PasswordErrorTextView);
        }

        private void PasswordEditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if(this.PasswordEditText.Text != this.PasswordEditText1.Text)
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
            if (this.PasswordEditText.Text != this.PasswordEditText1.Text)
            {
                View view = (View)sender;
                Snackbar.Make(view, "Passwords do not match.", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
                return;
            }

            if (
                this.NameEditText.Text == "" ||
                this.SurNameEditText.Text == "" ||
                this.LoginEditText.Text == "" ||
                this.EmailEditText.Text == "" ||
                this.PasswordEditText.Text == "")
            {
                this.ErrorTextView.Text = "Name\nPlease enter all the fields.";
            }

            if (!UserAPIConection.RegisterCustomer(
                this.NameEditText.Text,
                this.SurNameEditText.Text,
                this.LoginEditText.Text,
                this.EmailEditText.Text,
                this.PasswordEditText.Text))
            {
                this.ErrorTextView.Text = "This login already exists.";
            }

            StartActivity(typeof(HomeActivity));
        }
    }
}