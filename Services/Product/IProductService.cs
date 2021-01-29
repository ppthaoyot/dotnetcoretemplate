using System.Collections.Generic;
using System.Threading.Tasks;
using RPG_Project.DTOs;
using RPG_Project.Models;

namespace RPG_Project.Services.Product
{
    public interface IProductService
    {
        Task<ServiceResponse<List<GetProductGroupDto>>> GetAllProductGroup();
        Task<ServiceResponse<List<GetProductDto>>> GetAllProduct();

        Task<ServiceResponse<GetProductGroupDto>> GetProductGroupById(int productGroupId);

        Task<ServiceResponse<GetProductDto>> GetProductById(int productId);
    }
}