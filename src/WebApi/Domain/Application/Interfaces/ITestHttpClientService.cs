using Api.Test.Domain.Application.Entities;
using Api.Test.Domain.Application.Models;

namespace Api.Test.Domain.Application.Interfaces;

public interface ITestHttpClientService
{
    Task<List<User>> GetUsers();
    Task<User> GetUser(int id);
    Task<User> CreateUser(UserDto userDto);
    Task InsertDb(User user);
}