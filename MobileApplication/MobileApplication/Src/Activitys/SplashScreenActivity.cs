using System.Threading.Tasks;
using Android.App;
using Android.OS;
using MobileApplication.Activitys;
using MobileApplication.Src.API;

namespace MobileApplication.Src.Activitys
{
    [Activity(Label = "SplashScreenActivity", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true, Icon = "@drawable/icon")]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            UserAPIConection.LogOut();
            base.OnResume();
            Task startupWork = new Task(() => {
                StartActivity(typeof(HomeActivity));
            });
            startupWork.Start();
        }
    }
}