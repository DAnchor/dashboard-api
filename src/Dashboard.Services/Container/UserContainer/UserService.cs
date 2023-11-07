using AutoMapper;
using Dashboard.Core.Repositories;
using Dashboard.Core.Models;
using Dashboard.Dtos.Payload.User;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Services.Container.UserContainer;

public interface IUserService
{
    Task<IdentityResult> RegisterUser(UserRegistrationRequestDto request);
    Task<IEnumerable<UserModel>> ReadAllUsers();
    Task<UserModel> ReadUserById(string id);
    UserModel UpdateUser(UserModel user, UserUpdateDetailsRequestDto userUpdateDetailsDto);
    Task DeleteUser(UserModel user);
}

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ICrudRepository<UserModel> _crudRepository;
    private readonly UserManager<UserModel> _userManager;

    public UserService
    (
        IMapper mapper,
        ICrudRepository<UserModel> crudRepository,
        UserManager<UserModel> userManager
    )
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _crudRepository = crudRepository;
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<IdentityResult> RegisterUser(UserRegistrationRequestDto request)
    {
        var userMap = _mapper.Map<UserRegistrationRequestDto, UserModel>(request);

        var response = await _userManager.CreateAsync(userMap, request.Password);

        return response;
    }

    public async Task<IEnumerable<UserModel>> ReadAllUsers()
    {
        return await _crudRepository.ReadAll();
    }

    public async Task<UserModel> ReadUserById(string id)
    {
        return await _crudRepository.ReadById(id);
    }

    public UserModel UpdateUser(UserModel user, UserUpdateDetailsRequestDto request)
    {
        var userMap = _mapper.Map<UserUpdateDetailsRequestDto, UserModel>(request);

        _crudRepository.Update(userMap);

        return user;
    }

    public Task DeleteUser(UserModel user)
    {
        return _crudRepository.Delete(user);
    }
}