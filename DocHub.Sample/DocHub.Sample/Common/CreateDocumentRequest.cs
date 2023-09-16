namespace DocHub.Sample.Common;

public class CreateDocumentRequest
{
    public FileInfoRequest FileInfo { get; set; } = new();
    /// <summary>
    /// Tiêu đề chứng từ
    /// </summary>
    /// 
    public string Subject { get; set; }
    /// <summary>
    /// Mô tả
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Loại chứng từ (Id)
    /// </summary>
    /// 
    public int TypeId { get; set; }
    /// <summary>
    /// Mẫu chứng từ (Id)
    /// </summary>
    public int? DocumentTemplateId { get; set; }
    /// <summary>
    /// Bộ phân (Id)
    /// </summary>
    /// 
    public int DepartmentId { get; set; }
    /// <summary>
    /// Mã chứng từ
    /// </summary>
    /// 
    public string No { get; set; }
    /// <summary>
    /// Mã khách hàng
    /// </summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    /// Thông tin khách hàng
    /// </summary>
    public string? CustomerInformation { get; set; }
    /// <summary>
    /// Giá trị hợp đồng
    /// </summary>
    public decimal? ContractValue { get; set; }
    /// <summary>
    /// Có hiệu lực từ
    /// </summary>
    public DateTime? ValidFrom { get; set; }
    /// <summary>
    /// Có hiệu lực đến
    /// </summary>
    public DateTime? ValidTo { get; set; }
    /// <summary>
    /// Ngày hết hạn
    /// </summary>
    public DateTime? ExpiryDate { get; set; }
    /// <summary>
    /// File đính kèm (Id)
    /// </summary>
    public List<long> AttachmentIds { get; set; } = new();
    /// <summary>
    /// Chứng từ liên quan (Id)
    /// </summary>
    public List<Guid> RelatedDocumentIds { get; set; } = new();
    /// <summary>
    /// Người dùng được chia sẻ (Id)
    /// </summary>
    public List<int> SharedUserIds { get; set; } = new();
    /// <summary>
    /// Nhóm người dùng được chia sẻ (Id)
    /// </summary>
    public List<int> SharedUserGroupIds { get; set; } = new();
    /// <summary>
    /// Bộ phận được chia sẻ (Id)
    /// </summary>
    public List<int> SharedDepartmentIds { get; set; } = new();
    /// <summary>
    /// File đính kèm
    /// </summary>
    /// 

    public List<byte[]> Attachments { get; set; } = new();
}

public class FileInfoRequest
{
    public string? FilePath { get; set; }
    public byte[]? File { get; set; }
    public string FileName { get; set; }
}