using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPG_Project.Data;
using RPG_Project.DTOs;
using RPG_Project.DTOs.Product;
using RPG_Project.Models;

namespace RPG_Project.Services.Product
{
    public class ProductService : IProductService
    {
        #region Constructor
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _log;

        public ProductService(AppDBContext dBContext, IMapper mapper, ILogger<ProductService> log)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _log = log;
        }
        #endregion
        #region ProductGroup
        public async Task<ServiceResponse<List<GetProductGroupDto>>> GetAllProductGroup()
        {
            var productGroup = await _dBContext.ProductGroups
            .Include(x => x.Products)
            .AsNoTracking().ToListAsync();

            var dto = _mapper.Map<List<GetProductGroupDto>>(productGroup);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<GetProductGroupDto>> GetProductGroupById(int productGroupId)
        {
            var pg = await _dBContext.ProductGroups.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == productGroupId);

            if (pg == null)
            {
                return ResponseResult.Failure<GetProductGroupDto>("not found.");
            }

            var dto = _mapper.Map<GetProductGroupDto>(pg);

            return ResponseResult.Success(dto);
        }
        #endregion
        #region Product
        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProduct()
        {
            var product = await _dBContext.Products
             .Include(x => x.ProductGroup)
             .AsNoTracking()
             .ToListAsync();

            var dto = _mapper.Map<List<GetProductDto>>(product);

            return ResponseResult.Success(dto);
        }

        //OData test return without ServiceResponse
        public async Task<List<GetProductDto>> GetAllProduct2()
        {
            var product = await _dBContext.Products
             .Include(x => x.ProductGroup)
             .AsNoTracking()
             .ToListAsync();

            var dto = _mapper.Map<List<GetProductDto>>(product);

            return (dto);
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductById(int productId)
        {
            var p = await _dBContext.Products.Include(x => x.ProductGroup).FirstOrDefaultAsync(x => x.Id == productId);

            if (p == null)
            {
                return ResponseResult.Failure<GetProductDto>("not found.");
            }

            var dto = _mapper.Map<GetProductDto>(p);

            return ResponseResult.Success(dto);
        }
        public async Task<ServiceResponse<List<GetProductDto>>> AddProduct(AddProductDto newProduct)
        {
            try
            {
                _log.LogInformation("Start  process.");
                var product = await _dBContext.Products
                .Include(x => x.ProductGroup)
                .FirstOrDefaultAsync(x => x.Name == newProduct.Name);

                if (!(product is null))
                {
                    var msg = $"Duplicate name exists.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<List<GetProductDto>>(msg);
                }

                _log.LogInformation("Check ProductGroup.");
                var productGroup = await _dBContext.ProductGroups.FirstOrDefaultAsync(x => x.Id == newProduct.ProductGroupId);
                if (productGroup is null)
                {
                    var msg = $"Product Group not exists.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<List<GetProductDto>>(msg);
                }

                _log.LogInformation("Add New Product.");
                var addProduct = new Models.Product
                {
                    Name = newProduct.Name,
                    Price = newProduct.Price,
                    StockCount = newProduct.StockCount,
                    ProductGroupId = newProduct.ProductGroupId
                };

                _dBContext.Products.Add(addProduct);
                await _dBContext.SaveChangesAsync();
                _log.LogInformation("Success.");

                var productAll = await _dBContext.Products
                                .Include(x => x.ProductGroup)
                                .ToListAsync();

                var dto = _mapper.Map<List<GetProductDto>>(productAll);
                _log.LogInformation("End  process.");
                return ResponseResult.Success(dto);

            }
            catch (System.Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<List<GetProductDto>>(ex.Message);
            }
        }
        public async Task<ServiceResponse<GetProductDto>> UpdateProduct(ProductDto request)
        {
            try
            {
                _log.LogInformation("Start  process.");
                var product = await _dBContext.Products
                .Include(x => x.ProductGroup)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (product is null)
                {
                    var msg = $"This product id {request.Id} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<GetProductDto>(msg);
                }

                _log.LogInformation("Check Product Name.");

                var productName = await _dBContext.Products.FirstOrDefaultAsync(x => x.Name == request.Name.Trim());
                if (!(productName is null))
                {
                    var msg = $"Duplicate name exists.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<GetProductDto>(msg);
                }

                _log.LogInformation("Update Product.");
                product.Name = request.Name;
                product.Price = request.Price;
                product.StockCount = request.StockCount;

                _dBContext.Products.Update(product);
                await _dBContext.SaveChangesAsync();
                _log.LogInformation("Success.");

                var dto = _mapper.Map<GetProductDto>(product);
                _log.LogInformation("End  process.");
                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<GetProductDto>(ex.Message);
            }
        }
        public async Task<ServiceResponse<List<GetProductDto>>> RemoveProduct(int productId)
        {
            try
            {
                _log.LogInformation("Start  process.");
                var product = await _dBContext.Products
                .Include(x => x.ProductGroup)
                .FirstOrDefaultAsync(x => x.Id == productId);

                if (product is null)
                {
                    var msg = $"This product id {productId} not found.";
                    _log.LogError(msg);
                    return ResponseResult.Failure<List<GetProductDto>>(msg);
                }

                _log.LogInformation("Remove Product.");
                _dBContext.Products.Remove(product);
                await _dBContext.SaveChangesAsync();
                _log.LogInformation("Success.");

                var productAll = await _dBContext.Products
                                .Include(x => x.ProductGroup)
                                .ToListAsync();

                var dto = _mapper.Map<List<GetProductDto>>(productAll);
                _log.LogInformation("End  process.");
                return ResponseResult.Success(dto);
            }
            catch (System.Exception ex)
            {
                _log.LogError(ex.Message);
                return ResponseResult.Failure<List<GetProductDto>>(ex.Message);
            }
        }
        #endregion
    }
}