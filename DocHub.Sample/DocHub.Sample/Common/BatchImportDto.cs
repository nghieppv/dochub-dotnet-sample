namespace DocHub.Sample.Common;

public class BatchImportDto
{
    public int Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? UploadedByUserId { get; set; }
    public int NumberOfRecords { get; set; }
    public string? Name { get; set; }
    public List<DocumentDto> Documents { get; set; } = new();
}

public class DocumentDto
{
    public Guid Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public DateTime? DocumentDate { get; set; }
    public decimal? ContractValue { get; set; }
    public string? CustomerCode { get; set; }
    public string? CustomerInformation { get; set; }
    public string? No { get; set; }
    public string? Subject { get; set; }
    public string? DownloadUrl { get; set; }
}

public class CreateBatchImportDataDto
{
    public string? Name { get; set; }
    public int? DepartmentId { get; set; }
    public int? DocumentTypeId { get; set; }
    public int? DocumentTemplateId { get; set; }
    public List<string> Parameters { get; set; }
    public List<List<string>> Rows { get; set; }
}

public class DocumentTemplateDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<DocumentTemplatePlaceHolderDto>? FillableFields { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? FileName { get; set; }
    //public UserDto? CreatedByUser { get; set; }
    public string? DownloadUrl { get; set; }
    public string? PdfDownloadUrl { get; set; }
    public string? XlsxDownloadUrl { get; set; }
}

public class DocumentTemplatePlaceHolderDto
{
    //public int Id { get; set; }
    public string? Key { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    //public int DocumentTemplateId { get; set; }
}

public static class PlaceHolderKey
{
    public const string FileName = "{{D.FileName}}";
    public const string No = "{{D.No}}";
    public const string Subject = "{{D.Subject}}";
    public const string ExpiryDate = "{{D.ExpiryDate}}";
    public const string Description = "{{D.Description}}";
    public const string IsOrder = "{{D.IsOrder}}";

    public const string Code = "{{P.Code}}";
    public const string AccessPermission = "{{P.AccessPermission}}";
}

// Dùng thông tin này để mark placeholder và truyển cho đúng
// Thông tin này bên tích hợp nên cố định để tiện cho việc truyền và sử dụng
//public static class ExtraPlaceHolderKey
//{
//    public const string so_hop_dong = "{{so_hop_dong}}";
//    public const string so_danh_bo = "{{so_danh_bo}}";// Mã danh bộ
//    public const string ngay = "{{ngay}}"; // ngày ký hợp đồng
//    public const string thang = "{{thang}}";
//    public const string nam = "{{nam}}";
//    public const string ho_va_ten = "{{ho_va_ten}}";
//    public const string so_CMND = "{{so_CMND}}";
//    public const string ngay_cap_CMND = "{{ngay_cap_CMND}}";
//    public const string noi_thuong_thu = "{{noi_thuong_thu}}";
//    public const string dia_chi = "{{dia_chi}}";
//    public const string sdt = "{{sdt}}";
//    public const string fax = "{{fax}}";
//    public const string email = "{{email}}";
//    public const string so_tai_khoan = "{{so_tai_khoan}}";
//    public const string ngan_hang = "{{ngan_hang}}";
//    public const string co_dong_ho_nuoc = "{{co_dong_ho_nuoc}}";
//}