using System.Runtime.Serialization.Json;
using WireguardWeb.Core.Dto.User;
using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Managers;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.DomainServices;

public sealed class UserService<TRepository> 
    where TRepository : IRepository<User>, IUniqueNameRepository<User>
{
    public TRepository UserRepository { get; }

    public UserService(TRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public void CreateUser(CreateUserDto dto, IHasher<User> hasher)
    {
        if (dto.IsValid() == false)
            throw new Exception("Invalid data");
        
        if (UserRepository.CheckNameUniqueness(dto.UserName) == false)
            throw new Exception("Transferred UserName is not unique");

        var pwdHash = hasher.GetHash(dto.Password);
        var user = new User(UserRepository.GetNextId(), dto.UserName, pwdHash);
        UserRepository.Add(user);
    }

    public User GetUser(GetUserDto dto)
    {
        if (dto.IsValid() == false)
            throw new Exception("Invalid data");

        if (UserRepository.CheckNameUniqueness(dto.UserName))
            throw new Exception("Transferred UserName is not find");

        return UserRepository.GetByUniqueName(dto.UserName);
    }

    public User[] GetUsersInRange(GetUsersInRangeDto dto)
    {
        if (dto.IsValid() == false)
            throw new Exception("Invalid data");

        if (dto.StartIndex + dto.Count > UserRepository.Count)
            throw new Exception($"Transferred startIndex and count:{dto.StartIndex + dto.Count}" +
                                $" was more than there are count users:{UserRepository.Count}");

        return UserRepository.GetRange(dto.StartIndex, dto.Count);
    }
}