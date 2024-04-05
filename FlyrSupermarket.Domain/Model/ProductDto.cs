using System.ComponentModel.DataAnnotations;

namespace FlyrSupermarket.Domain.Model
{
    public class ProductDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
