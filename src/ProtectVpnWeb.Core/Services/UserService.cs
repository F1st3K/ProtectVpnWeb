using ProtectVpnWeb.Core.Dto.Connection;
using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Core.Services.Interfaces;

namespace ProtectVpnWeb.Core.Services;

public sealed class UserService<TRepository> : IUserService
    where TRepository : IRepository<User>, IUniqueNameRepository<User>,
    IManyRelationshipsRepository<User, Connection>
{
    private TRepository UserRepository { get; }

    public UserService(TRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public UserDto GetUser(string userName)
    {
        if (TryGetUser(userName, out var user) == false || user is null)
            throw new NotFoundException(new ExceptionParameter(userName, nameof(userName)));
        
        return user.ToTransfer();
    }
    
    public UserDto GetUser(int id)
    {
        if (TryGetUser(id, out var user) == false || user is null)
            throw new NotFoundException(new ExceptionParameter(id, nameof(id)));
        
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
        return Array.ConvertAll(users, input => input.ToTransfer());
    }

    public ConnectionDto[] GetConnectionsByUser(string userName)
    {
        if (TryGetUser(userName, out var user) == false || user is null)
            throw new NotFoundException(new ExceptionParameter(userName, nameof(userName)));
        
        var connections = UserRepository.GetRelatedEntities(user);
        return Array.ConvertAll(connections, input => input.ToTransfer());
    }
    
    public ConnectionDto[] GetConnectionsByUser(int id)
    {
        if (TryGetUser(id, out var user) == false || user is null)
            throw new NotFoundException(new ExceptionParameter(id, nameof(id)));
        
        var connections = UserRepository.GetRelatedEntities(user);
        return Array.ConvertAll(connections, input => input.ToTransfer());
    }

    public void CreateUser(CreateUserDto dto, IHashService hashService)
    {
        if (dto.UniqueName == string.Empty ||
            dto.Password == string.Empty ||
            Enum.TryParse(dto.Role, out UserRoles role) == false)
            throw new InvalidArgumentException(
                new ExceptionParameter(dto.UniqueName, nameof(dto.UniqueName)),
                new ExceptionParameter(dto.Password, nameof(dto.Password)),
                new ExceptionParameter(dto.Role, nameof(dto.Role)));

        var id = UserRepository.GetNextId();
        var user = new User(id, dto.UniqueName, hashService.GetHash(dto.Password), role);
        UserRepository.Add(user);
    }
    
    public void EditUser(string userName, UserDto dto)
    {
        if (TryGetUser(userName, out var user) == false || user is null)
            throw new NotFoundException(new ExceptionParameter(userName, nameof(userName)));
        
        if (dto.UniqueName == string.Empty ||
            dto.Id < 0 ||
            Enum.TryParse(dto.Role, out UserRoles _) == false)
            throw new InvalidArgumentException(
                new ExceptionParameter(dto.UniqueName, nameof(dto.UniqueName)),
                new ExceptionParameter(dto.Id, nameof(dto.Id)),
                new ExceptionParameter(dto.Role, nameof(dto.Role)));
        
        if (user.Id != dto.Id)
            throw new NonIdenticalException(
                new ExceptionParameter(user.Id, nameof(user.Id)),
                new ExceptionParameter(dto.Id, nameof(dto.Id)));
        
        user.ChangeOf(dto);
        UserRepository.Update(user);
    }
    
    public void EditUser(int id, UserDto dto)
    {
        if (TryGetUser(id, out var user) == false || user is null)
            throw new NotFoundException(new ExceptionParameter(id, nameof(id)));
        
        if (dto.UniqueName == string.Empty ||
            dto.Id < 0 ||
            Enum.TryParse(dto.Role, out UserRoles _) == false)
            throw new InvalidArgumentException(
                new ExceptionParameter(dto.UniqueName, nameof(dto.UniqueName)),
                new ExceptionParameter(dto.Id, nameof(dto.Id)),
                new ExceptionParameter(dto.Role, nameof(dto.Role)));
        
        if (user.Id != dto.Id)
            throw new NonIdenticalException(
                new ExceptionParameter(user.Id, nameof(user.Id)),
                new ExceptionParameter(dto.Id, nameof(dto.Id)));
        
        user.ChangeOf(dto);
        UserRepository.Update(user);
    }

    public void RemoveUser(int id)
    {
        if (TryGetUser(id, out _) == false)
            throw new NotFoundException(
                new ExceptionParameter(id, nameof(id)));
        
        UserRepository.Remove(id);
    }

    public void RemoveUser(string userName)
    {
        if (TryGetUser(userName, out _) == false)
            throw new NotFoundException(
                new ExceptionParameter(userName, nameof(userName)));
        
        UserRepository.Remove(userName);
    }

    private bool TryGetUser(int id, out User? user)
    {
        user = null;
        if (id < 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(id, nameof(id)));
        
        if (UserRepository.CheckIdUniqueness(id))
            return false;
        
        user = UserRepository.GetById(id);
        return true;
    }
    
    private bool TryGetUser(string uname, out User? user)
    {
        user = null;
        if (uname == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(uname, nameof(uname)));
        
        if (UserRepository.CheckNameUniqueness(uname))
            return false;
        
        user = UserRepository.GetByUniqueName(uname);
        return true;
    }
}