using System.Collections.Generic;
using System.Threading.Tasks;
using RPG_Project.DTOs;
using RPG_Project.DTOs.Product;
using RPG_Project.Models;

namespace RPG_Project.Services.Product
{
    public interface IProductService
    {
        #region ProductGroup
        Task<ServiceResponse<List<GetProductGroupDto>>> GetAllProductGroup();
        Task<ServiceResponse<GetProductGroupDto>> GetProductGroupById(int productGroupId);
        #endregion

        #region Product
        Task<ServiceResponse<List<GetProductDto>>> GetAllProduct();
        Task<List<GetProductDto>> GetAllProduct2();
        Task<ServiceResponse<GetProductDto>> GetProductById(int productId);
        Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct);
        Task<ServiceResponse<GetProductDto>> UpdateProduct(ProductDto request);
        Task<ServiceResponse<List<GetProductDto>>> RemoveProduct(int productId);
        #endregion

    }
}