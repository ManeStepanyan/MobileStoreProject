﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MobileApplication.Src.API;

namespace MobileApplication.Activitys
{
    [Activity(Label = "Mobile Store", Theme ="@style/AppTheme", MainLauncher = false)]
    class SignInActivity : Activity
    {
        private EditText LogInEditText;
        private EditText PasswordEditText;
        private TextView LogInTitleTextView;
        private Button SignInButton;
        private Button SignUpButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignInActivity);

            this.LogInEditText = FindViewById<EditText>(Resource.Id.login_signin);
            this.PasswordEditText = FindViewById<EditText>(Resource.Id.password_signin);
            this.LogInTitleTextView = FindViewById<TextView>(Resource.Id.tv_login_signin);

            this.SignInButton = FindViewById<Button>(Resource.Id.btn_sign_in);
            this.SignInButton.Click += SignInButton_Click;

            this.SignUpButton = FindViewById<Button>(Resource.Id.btn_sign_up);
            this.SignUpButton.Click += SignUpButton_Click;

            
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            var nextActivity = new Intent(this, typeof(SignUpCustomerActivity));
            StartActivity(nextActivity);
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            if (this.LogInEditText.Text == "" || this.PasswordEditText.Text == "")
            {
                this.LogInTitleTextView.Text = "Please enter all the fields.";
                return;
            }

            if (!UserAPIController.SigeIn(this.LogInEditText.Text, this.PasswordEditText.Text))
            {
                this.LogInTitleTextView.Text = "Wrong login or password.";
                return;
            }

            var nextActivity = new Intent(this, typeof(HomeActivity));
            StartActivity(nextActivity);
        }
    }
}