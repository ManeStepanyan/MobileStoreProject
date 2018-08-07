using MobileApplication.Src.Models;

namespace MobileApplication.Src.Activitys
{
    public static class ActivityCommunication
    {
        private static Product product;
        public static Product Product
        {
            get
            {
                var resoult = product;
                product = null;
                return resoult;
            }

            set
            {
                if (product != null)
                {
                    throw new System.Exception();
                }
                product = value;
            }
        }
        private static Seller seller;
        public static Seller Seller 
        {
            get
            {
                var resoult = seller;
                product = null;
                return seller;
            }

            set
            {
                if (seller != null)
                {
                    throw new System.Exception();
                }
                seller = value;
            }
        }
    }
}