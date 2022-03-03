using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(minimum: 2, maximum: 20)]
        public string Name { get; set; }
        [Required]
        [Range(minimum:2, maximum: 200)]
        public string Description { get; set; }
        public Nullable<decimal> Price { get; set; }
    }
}
