using System.Collections.Generic;
using System.Linq;

using Android.Widget;
using Java.Lang;

using MobileApplication.Src.ObjectExtensions;
using MobileApplication.Src.Models;
using Object = Java.Lang.Object;

namespace MobileApplication.Src.ListViewAdapters
{
    public partial class ProductsAdapter : BaseAdapter<Product>
    {
        public class ProductFilter : Filter
        {
            private readonly ProductsAdapter _adapter;
            public ProductFilter(ProductsAdapter adapter)
            {
                _adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<Product>();
                if (_adapter._originalData == null)
                    _adapter._originalData = _adapter.Products;

                if (constraint == null) return returnObj;

                if (_adapter._originalData != null && _adapter._originalData.Any())
                {
                    // Compare constraint to all names lowercased. 
                    // It they are contained they are added to results.
                    results.AddRange(
                        _adapter._originalData.Where(
                            chemical => chemical.Name.ToLower().Contains(constraint.ToString())));
                }

                // Nasty piece of .NET to Java wrapping, be careful with this!
                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    _adapter.Products = values.ToArray<Object>()
                        .Select(r => (r).ToNetObject<Product>()).ToList();

                _adapter.NotifyDataSetChanged();

                // Don't do this and see GREF counts rising
                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}