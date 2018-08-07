
using Android.App;
using Android.OS;
using MobileApplication;

namespace MobileApplication.Src.Activitys
{
    [Activity(Label = "BuyActivity")]
    public class BuyActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.BuyActivity);
            // Create your application here
        }
    }
}