using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.DTOs
{
    public class ProductDTO
    {
        //4.2.2 建立ProductDTO類別
        public string ProductID { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string Picture { get; set; } = null!;

        public string CateID { get; set; } = null!;

        public string CateName { get; set; } = null!;

    }
}
