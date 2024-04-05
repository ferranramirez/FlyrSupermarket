using System.ComponentModel.DataAnnotations;

namespace Flyr.Infrastructure.Model
{
    public class Product
    {
        [Key]
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
}
