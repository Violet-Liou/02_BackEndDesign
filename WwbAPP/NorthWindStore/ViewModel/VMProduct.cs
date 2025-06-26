using NorthWindStore.Models;

namespace NorthWindStore.ViewModel
{
    public class VMProduct
    {
        public List<Categories>? Categories { get; set; } // 產品類別資料
        public List<Products>? Products { get; set; } // 產品資料
    }
}
