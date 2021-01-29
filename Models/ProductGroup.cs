using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RPG_Project.Models
{
    [Table("ProductGroup")]
    public class ProductGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        // [JsonIgnore]
        public List<Product> Products { get; set; }
    }
}