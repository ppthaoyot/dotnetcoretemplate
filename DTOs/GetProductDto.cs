using RPG_Project.Models;

namespace RPG_Project.DTOs
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockCount { get; set; }
        public int ProductGroupId { get; set; }
    }
}