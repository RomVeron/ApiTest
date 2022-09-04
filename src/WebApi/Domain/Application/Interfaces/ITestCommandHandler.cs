using Api.Test.Domain.Application.Entities;
using Api.Test.Domain.Application.Models;

namespace Api.Test.Domain.Application.Interfaces;

public interface ITestCommandHandler
{
    Task<List<User>> Handle();
    Task<User> GetUser(int id);
    Task<User> CreateUser(UserDto userDto);
}