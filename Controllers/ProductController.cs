using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG_Project.DTOs.Product;
using RPG_Project.Services.Product;

namespace RPG_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        #region Constructor
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion


        #region ProductGroup

        [HttpGet("ProductGroup/getproductgroupall")]
        public async Task<IActionResult> GetAllProductGroup()
        {
            return Ok(await _productService.GetAllProductGroup());
        }

        [HttpGet("ProductGroup/{productGroupId}")]
        public async Task<IActionResult> GetProductGroupById(int productGroupId)
        {
            return Ok(await _productService.GetProductGroupById(productGroupId));
        }
        #endregion

        #region Product
        [HttpGet("getproductall")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(await _productService.GetAllProduct());
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            return Ok(await _productService.GetProductById(productId));
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> AddProduct(AddProductDto product)
        {
            return Ok(await _productService.AddProduct(product));
        }

        [HttpPut("updateproduct")]
        public async Task<IActionResult> UpdateProduct(ProductDto product)
        {
            return Ok(await _productService.UpdateProduct(product));
        }

        [HttpPut("removeproduct")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            return Ok(await _productService.RemoveProduct(productId));
        }
        #endregion
    }
}