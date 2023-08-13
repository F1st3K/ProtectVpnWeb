using WireguardWeb.Core.Dto.User;
using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Exceptions;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.DomainServices;

public sealed class UserService<TRepository> 
    where TRepository : IRepository<User>, IUniqueNameRepository<User>
{
    private TRepository UserRepository { get; }

    public UserService(TRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public UserDto GetUser(string userName)
    {
        if (userName == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(userName, nameof(userName)));

        if (UserRepository.CheckNameUniqueness(userName))
            throw new UserNameNotFoundException(
                new ExceptionParameter(userName, nameof(userName)));

        var user = UserRepository.GetByUniqueName(userName);

        return user.ToTransfer();
    }
    
    public UserDto GetUser(int id)
    {
        if (id < 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(id, nameof(id)));

        if (UserRepository.CheckIdUniqueness(id))
            throw new IdNotFoundException(
                new ExceptionParameter(id, nameof(id)));

        var user = UserRepository.GetById(id);

        return user.ToTransfer();
    }

    public UserDto[] GetUsersInRange(int startIndex, int count)
    {
        if (startIndex < 0 || count <= 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(startIndex, nameof(startIndex)),
                new ExceptionParameter(count, nameof(count)));

        if (startIndex + count > UserRepository.Count)
            throw new RangeException(
                new ExceptionParameter(startIndex+count, nameof(startIndex) + "+" + nameof(count)),
                new ExceptionParameter(UserRepository.Count, nameof(UserRepository.Count)));

        var users = UserRepository.GetRange(startIndex, count);
        
        var usersDto = new UserDto[users.Length];
        for (int i = 0; i < users.Length; i++)
        {
            usersDto[i] = users[i].ToTransfer();
        }
        return usersDto;
    }
    
    public void EditUser(string userName, UserDto dto)
    {
        if (userName == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(userName, nameof(userName)));

        if (UserRepository.CheckNameUniqueness(userName))
            throw new UserNameNotFoundException(
                new ExceptionParameter(userName, nameof(userName)));

        var user = UserRepository.GetByUniqueName(userName);
        
        if (user.Id != dto.Id)
            throw new NonIdenticalException(
                new ExceptionParameter(user.Id, nameof(user.Id)),
                new ExceptionParameter(dto.Id, nameof(dto.Id)));
        
        user.ChangeOf(dto);
        UserRepository.Update(user);
    }
    
    public void EditUser(int id, UserDto dto)
    {
        if (id < 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(id, nameof(id)));

        if (UserRepository.CheckIdUniqueness(id))
            throw new IdNotFoundException(
                new ExceptionParameter(id, nameof(id)));

        var user = UserRepository.GetById(id);

        if (user.Id != dto.Id)
            throw new NonIdenticalException(
                new ExceptionParameter(user.Id, nameof(user.Id)),
                new ExceptionParameter(dto.Id, nameof(dto.Id)));
        
        user.ChangeOf(dto);
        UserRepository.Update(user);
    }
}