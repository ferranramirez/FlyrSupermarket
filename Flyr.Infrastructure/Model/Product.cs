using System.ComponentModel.DataAnnotations;

namespace Flyr.Infrastructure.Model
{
    public class Product
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
