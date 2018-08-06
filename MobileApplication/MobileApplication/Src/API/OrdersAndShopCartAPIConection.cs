using System.Collections.Generic;

namespace MobileApplication.Src.API
{
    public static class OrdersAndShopCartAPIConection
    {
        private static List<int> CartDataBase;

        static OrdersAndShopCartAPIConection()
        {
            CartDataBase = new List<int>();
            //for (var i = 0; i < 10; ++i)
            //{
            //    CartDataBase.Add(i);
            //}
        }
        
        public static bool AddProduct(int id)
        {
            if (CartDataBase.Contains(id))
            {
                return false;
            }

            CartDataBase.Add(id);

            return true;
        }

        public static bool DeleteProduct(int id)
        {
            if (!CartDataBase.Contains(id))
            {
                return false;
            }

            CartDataBase.Remove(id);

            return true;
        }
        
        public static IEnumerable<int> GetAllProducts()
        {
            return CartDataBase;
        }
    }
}