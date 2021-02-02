namespace RPG_Project.DTOs
{
    public class BulkFilterDto : PaginationDto
    {

        //filter
        public string BulkName { get; set; }
        public string BulkCode { get; set; }

        //ordering
        public string OrderingField { get; set; }
        public bool AscendingOrder { get; set; } = true;
    }
}