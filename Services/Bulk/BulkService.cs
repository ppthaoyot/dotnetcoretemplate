using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RPG_Project.Data;
using RPG_Project.DTOs;
using RPG_Project.Helpers;
using RPG_Project.Models;
using System.Linq.Dynamic.Core;
using System;

namespace RPG_Project.Services
{
    public class BulkService : IBulkService
    {
        #region Constructor
        private readonly AppDBContext _dBContext;
        private readonly IHttpContextAccessor _httpContext;

        public BulkService(AppDBContext dBContext, IHttpContextAccessor httpContext)
        {
            _dBContext = dBContext;
            _httpContext = httpContext;
        }
        #endregion


        public List<Bulk> BulkInsert()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForInsert();
            _dBContext.BulkInsert(bulk);

            return bulk;
        }

        public List<Bulk> BulkUpdate()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForUpdate();
            _dBContext.BulkUpdate(bulk);

            return bulk;
        }

        public List<Bulk> BulkDelete()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = _dBContext.Bulk.ToList();
            _dBContext.BulkDelete(bulk);

            return bulk;
        }

        public List<Bulk> GetBulks()
        {
            return _dBContext.Bulk.ToList();
        }

        private static List<Bulk> GetDataForInsert()
        {
            List<Bulk> bulk = new List<Bulk>();
            for (int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkId = i + 1, BulkName = "Insert BulkName" + i, BulkCode = "Insert BulkCode" + i });
            }

            return bulk;
        }


        private static List<Bulk> GetDataForUpdate()
        {
            List<Bulk> bulk = new List<Bulk>();
            for (int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkId = i + 1, BulkName = "Update BulkName" + i, BulkCode = "Update BulkCode" + i });
            }

            return bulk;
        }

        public async Task<ServiceResponseWithPagination<List<Bulk>>> GetBulkWithPagination(PaginationDto pagination)
        {
            var queryable = _dBContext.Bulk.AsQueryable();

            var paginationResult = await _httpContext.HttpContext.InsertPaginationParametersInResponse(queryable, pagination.RecordsPerPage, pagination.Page);
            var dto = await queryable.Paginate(pagination).ToListAsync();

            return ResponseResultWithPagination.Success(dto, paginationResult);
        }

        public async Task<ServiceResponseWithPagination<List<Bulk>>> GetBulkFilter(BulkFilterDto filter)
        {
            var queryable = _dBContext.Bulk.AsQueryable();

            //Filter
            if (!string.IsNullOrWhiteSpace(filter.BulkName))
            {
                queryable = queryable.Where(x => x.BulkName.Contains(filter.BulkName));
            }

            if (!string.IsNullOrWhiteSpace(filter.BulkCode))
            {
                queryable = queryable.Where(x => x.BulkCode.Contains(filter.BulkCode));
            }

            //Ordering
            if (!string.IsNullOrWhiteSpace(filter.OrderingField))
            {
                try
                {
                    queryable = queryable.OrderBy($"{filter.OrderingField} {(filter.AscendingOrder ? "asc" : "desc")}");
                }
                catch (System.Exception)
                {
                    return ResponseResultWithPagination.Failure<List<Bulk>>(string.Format("Could not order by field : {0}", filter.OrderingField));
                }
            }

            var paginationResult = await _httpContext.HttpContext.InsertPaginationParametersInResponse(queryable, filter.RecordsPerPage, filter.Page);
            var dto = await queryable.Paginate(filter).ToListAsync();

            return ResponseResultWithPagination.Success(dto, paginationResult);
        }

        public async Task<ServiceResponse<List<Bulk>>> GetBulksByInlineSQL(int bulkId)
        {
            var result = await _dBContext.Bulk.FromSqlRaw($"Select * from RPG_DB.dbo.[Bulk] where BulkId = {bulkId}").ToListAsync();
            return ResponseResult.Success(result);
        }

        public async Task<ServiceResponse<List<Bulk>>> GetBulksByStoreProcedure(int bulkId)
        {
            var result = await _dBContext.Bulk.FromSqlRaw($"exec dbo.usp_BulkById_select @BulkId ={bulkId}").ToListAsync();
            return ResponseResult.Success(result);
        }

        public string BulkTrans()
        {
            string resultMsg = "success";

            using (var transaction = _dBContext.Database.BeginTransaction())
            {
                try
                {
                    List<Bulk> bulk = new List<Bulk>();
                    bulk = GetDataForInsert();
                    _dBContext.BulkInsert(bulk);

                    //_dbContext.BulkInsert(bulk);

                    bulk = GetDataForUpdate();
                    _dBContext.BulkUpdate(bulk);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    resultMsg = ex.Message;
                }
            }
            //insert
            //update
            return resultMsg;
        }
    }
}