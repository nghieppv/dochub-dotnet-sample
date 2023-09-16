using DocHub.Sample.Common;
using DocHub.Sample.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;

namespace DocHub.Sample;

class Program
{
    public async static Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args).Build();
        IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

        var appConfig = config.GetSection("AppConfiguration").Get<AppConfiguration>();

        //Tạo chứng từ từ mẫu
        //await BatchImport(appConfig);

        //Tạo chứng từ từ tệp tin pdf
        await CreateDocument(appConfig);
    }

    /// <summary>
    /// Tạo chứng từ từ mẫu
    /// </summary>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    private async static Task BatchImport(AppConfiguration appConfig)
    {
        var username = appConfig.UserName;
        var password = appConfig.Password;
        var comId = appConfig.CompanyId;
        var baseUrl = appConfig.BaseUrl;

        // Site 213
        var documentTemplateId = 1141; // lấy từ api danh sách mẫu chứng từ (/api/document-templates)
        var documentTypeId = 1089;// lấy từ api danh sách loại chứng từ (/api/document-types)
        var departmentId = 33;// lấy từ api danh sách bộ phận (/api/departments)
        var userCode = "baoth";

        // Dữ liệu mẫu đã chuẩn bị cho CÔNG TY CỔ PHẦN CẤP NƯỚC CHỢ LỚN
        // var documentTemplateId = 4064; // lấy từ api danh sách mẫu chứng từ (/api/document-templates)
        // var documentTypeId = 9162;// lấy từ api danh sách loại chứng từ (/api/document-types)
        // var departmentId = 6185;// lấy từ api danh sách bộ phận (/api/departments)
        // var userCode = "CNCL.DEMO";

        var docHubService = new DocHubService();

        // Xác thực
        var authResult = await docHubService.AuthenticateAsync(username, password, comId, baseUrl);
        Console.WriteLine(authResult.Messages[0]);

        // Lấy thông tin mẫu => FillableFields
        //var documentTemplate = await docHubService.GetDocumentTemplateAsync(documentTemplateId);

        // Begin - Chuẩn bị dữ liệu import
        var batchImportData = new CreateBatchImportDataDto();

        //batchImportData.Name = "Hợp đồng tháng 12 trình GĐ ký"; // set null nếu muốn tự động sinh
        batchImportData.DocumentTemplateId = documentTemplateId;
        batchImportData.DocumentTypeId = documentTypeId;
        batchImportData.DepartmentId = departmentId;


        // parameters - các tham số - các tham số phải đúng thứ tự
        // 6 tham số đầu là bắt buộc - ko được thay đổi thứ tự
        var parameters = new List<string>
        {
            PlaceHolderKey.FileName,
            PlaceHolderKey.No,
            PlaceHolderKey.Subject,
            PlaceHolderKey.ExpiryDate,
            PlaceHolderKey.Description,
            PlaceHolderKey.IsOrder
        };


        // danh sách người nhận - không được thay đổi thứ tự - gồm người xử lý + quyền truy cập
        // VD: quy trình gồm 2 người xử lý
        for (int i = 1; i <= 2; i++)
        {
            parameters.AddRange(new List<string> { PlaceHolderKey.Code, PlaceHolderKey.AccessPermission });
        }

        // tham số phụ => lấy từ api lấy thông tin mẫu chứng từ (/api/document-templates/{id})
        parameters.Add("{{day}}");
        parameters.Add("{{month}}");
        parameters.Add("{{year}}");
        parameters.Add("{{ben_a}}");
        parameters.Add("{{ben_b}}");
        parameters.Add("{{dien_tich_dat}}");
        parameters.Add("{{dien_tich_nha}}");


        // Dữ liệu từng hàng cần fill
        // Ví dụ cần tạo 2 chứng từ
        var rows = new List<List<string>>();
        var rdDocumentNo = Guid.NewGuid().ToString()[..4].ToUpper();
        for (int i = 1; i <= 50; i++)
        {
            var row = new List<string>
            {
                $"{rdDocumentNo}{i}", // tên tệp tin
                $"{rdDocumentNo}{i}", // mã chứng từ không được phép trùng
                $"Hợp đồng {rdDocumentNo}{i}", //tên chứng từ
                "", //ngày hết hạn - định dạng dd/MM/yyyy (vd: 20/11/2022)
                "Chứng từ thử nghiệm", //mô tả (nếu có)
                "Y" //xử lý tuần tự (Y/N)
            };

            // quy trình ký - số lượng người phải khớp với tham số phía trên - quy trình gồm 2 người thì phải thêm đủ 2 người
            row.AddRange(new List<string> { userCode, "DR" });
            row.AddRange(new List<string> { userCode, "D" });

            // Dữ liệu fill => truyền đúng vị trí ứng với tham số bên trên
            row.Add(DateTime.Now.Day.ToString());
            row.Add(DateTime.Now.Month.ToString());
            row.Add(DateTime.Now.Year.ToString());
            row.Add($"Nguyễn Văn A_{i}");
            row.Add($"Nguyễn Văn B_{i}");
            row.Add("1000");
            row.Add("700");

            rows.Add(row);
        }

        batchImportData.Parameters = parameters;
        batchImportData.Rows = rows;
        // End - Chuẩn bị dữ liệu import

        // Tạo lô chứng từ
        var createResult = await docHubService.CreateBatchImportAsync(batchImportData);
        Console.WriteLine(createResult.Messages[0]);

        // Gửi quy trình ký
        //if (createResult.Success && createResult.Data != null)
        //{
        //    var batchId = createResult.Data.Id;
        //    var sendResult = await docHubService.SendBatchDocumentAsync(batchId);
        //    Console.WriteLine(sendResult.Messages[0]);
        //}

        Console.ReadLine();
    }

    /// <summary>
    /// Tạo chứng từ từ tệp tin pdf
    /// </summary>
    /// <param name="appConfig"></param>
    /// <returns></returns>
    private async static Task CreateDocument(AppConfiguration appConfig)
    {
        var username = appConfig.UserName;
        var password = appConfig.Password;
        var comId = appConfig.CompanyId;
        var baseUrl = appConfig.BaseUrl;

        // Site 213
        var documentTypeId = 54;// lấy từ api danh sách loại chứng từ (/api/document-types)
        var departmentId = 33;// lấy từ api danh sách bộ phận (/api/departments)

        var docHubService = new DocHubService();

        // Xác thực
        var authResult = await docHubService.AuthenticateAsync(username, password, comId, baseUrl);
        Console.WriteLine(authResult.Messages[0]);

        // Tạo mới chứng từ
        var randomText = Guid.NewGuid().ToString()[..4].ToUpper();
        var request = new CreateDocumentRequest();
        request.TypeId = documentTypeId;
        request.DepartmentId = departmentId;
        request.No = randomText;
        request.Subject = "Chứng từ thử nghiệm " + randomText;
        request.Description = "Chứng từ thử nghiệm";

        request.FileInfo.FileName = "sample.pdf";

        //---------------------------------
        // Trường hợp sử dụng đường dẫn tệp tin chứng từ cố định
        request.FileInfo.FilePath = "./sample.pdf";
        //---------------------------------

        //---------------------------------
        // Trường hợp sử dụng base64 của tệp tin chứng từ
        //var base64String = System.IO.File.ReadAllText("./base64String.txt");
        //request.FileInfo.File = System.Convert.FromBase64String(base64String);
        //---------------------------------

        var createResult = await docHubService.CreateDocumentAsync(request);
        Console.WriteLine(createResult.Messages[0]);
    }
}



