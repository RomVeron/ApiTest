using System.Net.Http.Headers;
using System.Text;
using Api.Test.Domain.Application.Data;
using Api.Test.Domain.Application.Entities;
using Api.Test.Domain.Application.Interfaces;
using Api.Test.Domain.Application.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Api.Test.Domain.Application.Services;

public class TestHttpClientService : ITestHttpClientService
{
    private readonly ILogger<TestHttpClientService> _logger;
    private readonly HttpClient _client;
    private readonly string _endPoint;
    private readonly ApiDbContext _db;
    
    public TestHttpClientService(IConfiguration iConfig, HttpClient client, ILogger<TestHttpClientService> logger, ApiDbContext db)
    {
        _logger = logger;
        _client = client;
        _endPoint = iConfig["RequestVariable:EndPoint"];
        _db = db;
    }

    public async Task<List<User>> GetUsers()
    {
        _logger.LogDebug("Ingreso ObtenerTest");
        _client.BaseAddress = new Uri(_endPoint);
        _logger.LogDebug("base adress {address}", _client.BaseAddress);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        
        _logger.LogDebug("Before get");
        var response = _client.GetAsync(_endPoint+"users").GetAwaiter().GetResult();
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest){
            
        }
        _logger.LogDebug("response: {@Respuesta}", response);
        var resultContent = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<List<User>>(resultContent);
    }
    
    public async Task<User> GetUser(int id)
    {
        _logger.LogDebug("Ingreso ObtenerTest");
        _client.BaseAddress = new Uri(_endPoint);
        _logger.LogDebug("base adress {address}", _client.BaseAddress);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        
        _logger.LogDebug("Before get");
        var response = _client.GetAsync(_endPoint+"users/"+id).GetAwaiter().GetResult();
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest){
            
        }
        _logger.LogDebug("response: {@Respuesta}", response);
        var resultContent = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<User>(resultContent);
    }
    
    public async Task<User> CreateUser(UserDto userDto)
    {
        _logger.LogDebug("Ingreso ObtenerTest");
        _client.BaseAddress = new Uri(_endPoint);
        _logger.LogDebug("base adress {address}", _client.BaseAddress);
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        
        var json = JsonSerializer.Serialize(userDto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = _client.PostAsync(_endPoint+"users", content).GetAwaiter().GetResult();
        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest){
            
        }
        _logger.LogDebug("response: {@Respuesta}", response);
        var resultContent = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<User>(resultContent);
    }

    public async Task InsertDb(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }
}