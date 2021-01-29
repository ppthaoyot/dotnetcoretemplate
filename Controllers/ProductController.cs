using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG_Project.Services.Product;

namespace RPG_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet("getproductgroupall")]
        public async Task<IActionResult> GetAllProductGroup()
        {
            return Ok(await _productService.GetAllProductGroup());
        }

        [HttpGet("getproductall")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(await _productService.GetAllProduct());
        }

        [HttpGet("pg/{productGroupId}")]
        public async Task<IActionResult> GetProductGroupById(int productGroupId)
        {
            return Ok(await _productService.GetProductGroupById(productGroupId));
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            return Ok(await _productService.GetProductById(productId));
        }
    }
}