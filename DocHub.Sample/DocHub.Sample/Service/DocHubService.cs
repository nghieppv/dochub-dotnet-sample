using DocHub.Sample.Common;
using Newtonsoft.Json;
using System.Text;

namespace DocHub.Sample.Service;

public interface IDocHubService
{
    public Task<Result<string>> AuthenticateAsync(string? username, string? password, string? comId, string? baseUrl);
    public Task<Result<BatchImportDto>> CreateBatchImportAsync(CreateBatchImportDataDto dto);
    public Task<Result<BatchImportDto>> SendBatchDocumentAsync(int id);
    public Task<Result<DocumentTemplateDto>> GetDocumentTemplateAsync(int id);
    public Task<Result<DocumentDto>> CreateDocumentAsync(CreateDocumentRequest request);
}

public class DocHubService : IDocHubService
{
    private string? _accessToken;
    private string? _baseUrl;
    public async Task<Result<string>> AuthenticateAsync(string? username, string? password, string? comId, string? baseUrl)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(baseUrl))
        {
            return new();
        }
        _baseUrl = baseUrl;
        var httpClient = new HttpClient();
        var httpContent = new StringContent(JsonConvert.SerializeObject(new
        {
            username = username,
            password = password,
            companyId = comId

        }), Encoding.UTF8, "application/json");
        var r = await httpClient.PostAsync(_baseUrl + "/api/auth/password-login", httpContent);
        var content = await r.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Result<string>>(content);

        if (response == null) return new("Đăng nhập thất bại");

        if (response.Success)
        {
            _accessToken = response.Data;
        }

        return response;
    }

    public async Task<Result<BatchImportDto>> CreateBatchImportAsync(CreateBatchImportDataDto dto)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        var httpContent = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        var r = await httpClient.PostAsync(_baseUrl + "/api/batch-imports/create-advanced", httpContent);
        var content = await r.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Result<BatchImportDto>>(content);

        return response ?? new("Thất bại");
    }

    public async Task<Result<BatchImportDto>> SendBatchDocumentAsync(int id)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        var r = await httpClient.PostAsync(_baseUrl + "/api/batch-imports/send/" + id, null);
        var content = await r.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Result<BatchImportDto>>(content);
        return response ?? new("Thất bại");
    }

    public async Task<Result<DocumentTemplateDto>> GetDocumentTemplateAsync(int id)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
        var r = await httpClient.GetAsync(_baseUrl + "/api/document-templates/" + id);
        var content = await r.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Result<DocumentTemplateDto>>(content);
        return response ?? new("Thất bại");
    }

    public async Task<Result<DocumentDto>> CreateDocumentAsync(CreateDocumentRequest request)
    {

        var httpClient = new HttpClient();
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "/api/documents/create");
        httpRequest.Headers.Add("Authorization", "Bearer " + _accessToken);


        var content = new MultipartFormDataContent();
        content.Add(new StringContent(request.No), "No");
        content.Add(new StringContent(request.Subject), "Subject");
        content.Add(new StringContent(request.Description), "Description");
        content.Add(new StringContent(request.TypeId.ToString()), "TypeId");
        content.Add(new StringContent(request.DepartmentId.ToString()), "DepartmentId");

        if (!string.IsNullOrEmpty(request.FileInfo.FilePath))
        {
            content.Add(new StreamContent(File.OpenRead(request.FileInfo.FilePath)), "File", request.FileInfo.FileName);
        }
        else
        {
            content.Add(new ByteArrayContent(request.FileInfo.File), "File", request.FileInfo.FileName);
        }
        

        httpRequest.Content = content;
        var r = await httpClient.SendAsync(httpRequest);
        r.EnsureSuccessStatusCode();

        var httpContent = await r.Content.ReadAsStringAsync();

        var response = JsonConvert.DeserializeObject<Result<DocumentDto>>(httpContent);

        return response ?? new("Thất bại");
    }
}
