using System;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPG_Project.DTOs.Product;
using RPG_Project.Services.Product;

namespace RPG_Project.Controllers
{
    [ApiController]
    [Route("api/products")]
    [ODataRoutePrefix("Product")]
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
        [HttpGet("all")]
        [EnableQuery]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(await _productService.GetAllProduct());
        }

        [HttpGet("odata/all")]
        [EnableQuery]
        public async Task<IActionResult> GetAllProduct2()
        {
            return Ok(await _productService.GetAllProduct2());
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            return Ok(await _productService.GetProductById(productId));
        }

        [HttpPost()]
        public async Task<IActionResult> AddProduct(AddProductDto product)
        {
            return Ok(await _productService.AddProduct(product));
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateProduct(ProductDto product)
        {
            return Ok(await _productService.UpdateProduct(product));
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveProduct(int productId)
        {
            return Ok(await _productService.RemoveProduct(productId));
        }
        #endregion
    }
}