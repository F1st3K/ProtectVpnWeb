using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Repositories;

namespace ProtectVpnWeb.Core.Services.Implementations;

public sealed class AuthService<TUserRepository, TRefreshTokenRepository, TTokenService, THasher>
    : IAuthService
    where TUserRepository : IRepository<User>, IUniqueNameRepository<User>
    where TRefreshTokenRepository : ITokenRepository<string>
    where TTokenService : ITokenService<string>
    where THasher : IHashService
{
    private TUserRepository UserRepository { get; }
    
    private TRefreshTokenRepository RefreshTokenRepository { get; }
    
    private TTokenService TokenService { get; }
    
    private THasher Hasher { get; }
    
    public AuthService(
        TUserRepository userRepository,
        TRefreshTokenRepository refreshTokenRepository,
        TTokenService tokenService,
        THasher hasher)
    {
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
        TokenService = tokenService;
        Hasher = hasher;
    }
    
    public string RegisterUser(AuthUserDto dto)
    {
        if (dto.Password == string.Empty ||
            dto.UserName == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(dto.Password, nameof(dto.Password)),
                new ExceptionParameter(dto.UserName, nameof(dto.UserName)));

        if (UserRepository.CheckNameUniqueness(dto.UserName) == false)
            throw new DuplicateUniqKeyException(
                new ExceptionParameter(dto.UserName, nameof(dto.UserName)));

        var user = new User(
            UserRepository.GetNextId(),
            dto.UserName,
            Hasher.GetHash(dto.Password),
            null
        );
        UserRepository.Add(user);
        
        return AuthUser(dto);
    }

    public string AuthUser(AuthUserDto dto)
    {
        if (dto.Password == string.Empty ||
            dto.UserName == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(dto.Password, nameof(dto.Password)),
                new ExceptionParameter(dto.UserName, nameof(dto.UserName)));

        if (UserRepository.CheckNameUniqueness(dto.UserName))
            throw new InvalidAuthenticationException();
        
        var user = UserRepository.GetByUniqueName(dto.UserName);
        if (user.HashPassword != Hasher.GetHash(dto.Password))
            throw new InvalidAuthenticationException();

        var refresh = TokenService.GenerateToken();
        RefreshTokenRepository.AddToken(refresh);
        return refresh;
    }

    public string ChangePassword(ChangePwdDto dto)
    {
        if (dto.Password == string.Empty ||
            dto.NewPassword == string.Empty ||
            dto.UserName == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(dto.Password, nameof(dto.Password)),
                new ExceptionParameter(dto.NewPassword, nameof(dto.NewPassword)),
                new ExceptionParameter(dto.UserName, nameof(dto.UserName)));

        if (dto.Password == dto.NewPassword)
            throw new ParamsMatchException(
                new ExceptionParameter(dto.Password, nameof(dto.Password)),
                new ExceptionParameter(dto.NewPassword, nameof(dto.NewPassword)));

        AuthUser(new AuthUserDto { UserName = dto.UserName, Password = dto.NewPassword });
        var user = UserRepository.GetByUniqueName(dto.UserName);
        var editUser = new User(user.Id, user.UniqueName, 
            Hasher.GetHash(dto.NewPassword), user.Role);
        UserRepository.Update(editUser);

        return AuthUser(new AuthUserDto { UserName = dto.UserName, Password = dto.NewPassword });
    }

    public void GetTokensByRefreshToken(string token, out string refreshToken, out string accessToken)
    {
        if (token == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(token, nameof(refreshToken)));

        if (RefreshTokenRepository.TokenExists(token) == false)
            throw new NotFoundException(
                new ExceptionParameter(token, nameof(refreshToken)));

        RefreshTokenRepository.RemoveToken(token);
        refreshToken = TokenService.GenerateToken();
        accessToken = TokenService.GenerateToken();
        RefreshTokenRepository.AddToken(refreshToken);
    }

    public void RemoveRefreshToken(string token)
    {
        if (token == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(token, nameof(token)));

        if (RefreshTokenRepository.TokenExists(token) == false)
            throw new NotFoundException(
                new ExceptionParameter(token, nameof(token)));
        
        RefreshTokenRepository.RemoveToken(token);
    }

    public bool ValidateAccessToken(string token, out UserRoles? role)
    {
        role = null;
        return false;
    }
}