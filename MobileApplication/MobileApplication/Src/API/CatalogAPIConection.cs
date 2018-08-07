using System.Collections.Generic;

namespace MobileApplication.Src.API
{
    public static class CatalogAPIConection
    {
        private static Dictionary<int, List<int>> CatalogDataBase;

        static CatalogAPIConection()
        {
            CatalogDataBase = new Dictionary<int, List<int>>();

        }

        public static int GetSellerIdByProductId(int id)
        {
            return 1;
        }

        public static void AddProduct(int sellerId, int productId)
        {
            if (CatalogDataBase.ContainsKey(sellerId))
            {
                CatalogDataBase[sellerId].Add(productId);
            }
            else
            {
                CatalogDataBase.Add(sellerId, new List<int>() { productId });
            }
        }

        public static IEnumerable<int> GetProductsBySellerId(int sellerId)
        {
            return CatalogDataBase[sellerId];
        }
    }
}