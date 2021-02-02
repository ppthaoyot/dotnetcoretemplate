using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using RPG_Project.DTOs;
using RPG_Project.Services;

namespace RPG_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ODataRoutePrefix("Bulk")]
    public class BulkController : ControllerBase
    {
        #region Constructor
        private readonly IBulkService _bulkService;

        public BulkController(IBulkService bulkService)
        {
            _bulkService = bulkService;
        }

        #endregion

        [HttpGet("all")]
        [EnableQuery]
        public IActionResult GetBulk()
        {

            return Ok(_bulkService.GetBulks());
        }

        [HttpPost("insert")]
        public IActionResult BulkInsert()
        {

            return Ok(_bulkService.BulkInsert());
        }

        [HttpPut("update")]
        public IActionResult BulkUpdate()
        {
            return Ok(_bulkService.BulkUpdate());
        }

        [HttpDelete("delete")]
        public IActionResult BulkDelete()
        {
            return Ok(_bulkService.BulkDelete());
        }

        [HttpGet("bulk/pagination")]
        public async Task<IActionResult> GetBulkWithPagination([FromQuery] PaginationDto pagination)
        {
            return Ok(await _bulkService.GetBulkWithPagination(pagination));
        }

        [HttpGet("bulk/filter")]
        public async Task<IActionResult> GetBulkFilter([FromQuery] BulkFilterDto filter)
        {
            return Ok(await _bulkService.GetBulkFilter(filter));
        }

        [HttpGet("bulk/inline")]
        public async Task<IActionResult> GetBulkByInlineSQL(int bulkId)
        {
            return Ok(await _bulkService.GetBulksByInlineSQL(bulkId));
        }
        [HttpGet("bulk/storeprocedure")]
        public async Task<IActionResult> GetBulkByStoreProcedure(int bulkId)
        {
            return Ok(await _bulkService.GetBulksByStoreProcedure(bulkId));
        }

        [HttpGet("bulk/trans")]
        public IActionResult BulkTrans()
        {
            return Ok(_bulkService.BulkTrans());
        }
    }
}