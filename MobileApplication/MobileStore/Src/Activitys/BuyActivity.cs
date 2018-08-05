
using Android.App;
using Android.OS;

namespace MobileStore.Src.Activitys
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