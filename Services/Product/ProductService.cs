using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPG_Project.Data;
using RPG_Project.DTOs;
using RPG_Project.Models;

namespace RPG_Project.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly AppDBContext _dBContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _log;

        public ProductService(AppDBContext dBContext, IMapper mapper, ILogger<ProductService> log)
        {
            _dBContext = dBContext;
            _mapper = mapper;
            _log = log;
        }


        public async Task<ServiceResponse<List<GetProductGroupDto>>> GetAllProductGroup()
        {
            var productGroup = await _dBContext.ProductGroups
            .Include(x => x.Products)
            .AsNoTracking().ToListAsync();

            var dto = _mapper.Map<List<GetProductGroupDto>>(productGroup);

            return ResponseResult.Success(dto);
        }

        public async Task<ServiceResponse<List<GetProductDto>>> GetAllProduct()
        {
            var product = await _dBContext.Products
             .Include(x => x.ProductGroup)
             .AsNoTracking().ToListAsync();

            var dto = _mapper.Map<List<GetProductDto>>(product);

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
    }
}