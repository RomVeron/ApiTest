using Api.Test.Domain.Application.Entities;
using Api.Test.Domain.Application.Interfaces;
using Api.Test.Domain.Application.Models;

namespace Api.Test.Domain.Application.Commands
{
    public class TestCommandHandler : ITestCommandHandler
    {
        private readonly ILogger<TestCommandHandler> _logger;
        private readonly ITestHttpClientService _httpClient;
        
        public TestCommandHandler(ILogger<TestCommandHandler> logger, ITestHttpClientService httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public Task<List<User>> Handle()
        {
            var response = _httpClient.GetUsers();

            return response;
        }

        public Task<User> GetUser(int id)
        {
            var response = _httpClient.GetUser(id);

            return response;
        }

        public Task<User> CreateUser(UserDto userDto)
        {
            var response = _httpClient.CreateUser(userDto);
            
            _httpClient.InsertDb(response.Result);

            return response;
        }
    }
}