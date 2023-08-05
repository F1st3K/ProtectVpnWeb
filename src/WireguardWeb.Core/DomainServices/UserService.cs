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
        if (dto.UserName == string.Empty || dto.Password == string.Empty)
            throw new Exception("Invalid data");
        
        if (UserRepository.CheckNameUniqueness(dto.UserName) == false)
            throw new Exception("Transferred UserName is not unique");

        var pwdHash = hasher.GetHash(dto.Password);
        var user = new User(UserRepository.GetNextId(), dto.UserName, pwdHash);
        UserRepository.Add(user);
    }

    public UserDto GetUser(string userName)
    {
        if (userName == string.Empty)
            throw new Exception("Invalid data");

        if (UserRepository.CheckNameUniqueness(userName))
            throw new Exception("Transferred UserName is not find");

        var user = UserRepository.GetByUniqueName(userName);

        return user.ToDto();
    }
    
    public UserDto GetUser(int id)
    {
        if (id < 0)
            throw new Exception("Invalid data");

        if (UserRepository.CheckIdUniqueness(id))
            throw new Exception("Transferred Id is not find");

        var user = UserRepository.GetById(id);

        return user.ToDto();
    }

    public UserDto[] GetUsersInRange(int startIndex, int count)
    {
        if (startIndex < 0 || count <= 0)
            throw new Exception("Invalid data");

        if (startIndex + count > UserRepository.Count)
            throw new Exception($"Transferred startIndex and count:{startIndex + count}" +
                                $" was more than there are count users:{UserRepository.Count}");

        var users = UserRepository.GetRange(startIndex, count);
        
        var usersDto = new UserDto[users.Length];
        for (int i = 0; i < users.Length; i++)
        {
            usersDto[i] = users[i].ToDto();
        }
        return usersDto;
    }
    
    
}