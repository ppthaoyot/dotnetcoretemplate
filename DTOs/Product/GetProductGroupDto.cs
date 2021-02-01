using System.Collections.Generic;
using RPG_Project.Models;

namespace RPG_Project.DTOs.Product
{
    public class GetProductGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetProductDto> Products { get; set; }
    }
}