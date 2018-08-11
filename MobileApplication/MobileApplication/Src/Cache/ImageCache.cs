using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MobileApplication.Src.Cache
{
    public static class ImageCache
    {
        public  static Dictionary<string, Bitmap> Cache { get; private set; }

        static ImageCache()
        {
            Cache = new Dictionary<string, Bitmap>();
        }
    }
}