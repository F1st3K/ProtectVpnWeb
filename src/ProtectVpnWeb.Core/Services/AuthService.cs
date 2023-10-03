using ProtectVpnWeb.Core.Dto.Auth;
using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Core.Services.Interfaces;

namespace ProtectVpnWeb.Core.Services;

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
    
    private TimeLiveTokensDto TimeLiveTokens { get; }
    
    private THasher Hasher { get; }
    
    public AuthService(TUserRepository userRepository,
        TRefreshTokenRepository refreshTokenRepository,
        TTokenService tokenService,
        THasher hasher,
        TimeLiveTokensDto timeLiveTokens)
    {
        UserRepository = userRepository;
        RefreshTokenRepository = refreshTokenRepository;
        TokenService = tokenService;
        TimeLiveTokens = timeLiveTokens;
        Hasher = hasher;
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
        
        var user = UserRepository.Get(dto.UserName);
        if (user.HashPassword != Hasher.GetHash(dto.Password))
            throw new InvalidAuthenticationException();
        
        var refresh = TokenService.GenerateToken(
            new UserIdDto{ Id = user.Id}, TimeLiveTokens.RefreshAuthToken);
        RefreshTokenRepository.AddToken(refresh);
        return refresh;
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
            Hasher.GetHash(dto.Password)
        );
        UserRepository.Add(user);
        
        return AuthUser(dto);
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

        if (UserRepository.CheckNameUniqueness(dto.UserName))
            throw new InvalidAuthenticationException();

        var user = UserRepository.Get(dto.UserName);
        if (user.HashPassword != Hasher.GetHash(dto.Password))
            throw new InvalidAuthenticationException();
        
        var editUser = new User(user.Id, user.UniqueName, 
            Hasher.GetHash(dto.NewPassword), user.Role);
        UserRepository.Update(editUser);

        return AuthUser(new AuthUserDto { UserName = dto.UserName, Password = dto.NewPassword });
    }

    public void GetTokensByRefreshToken(
        string token, out string refreshToken, out string accessToken)
    {
        if (token == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(token, nameof(refreshToken)));

        if (RefreshTokenRepository.TokenExists(token) == false)
            throw new NotFoundException(
                new ExceptionParameter(token, nameof(refreshToken)));
        RefreshTokenRepository.RemoveToken(token);

        if (TokenService.ValidateToken(token) == false)
            throw new InvalidTokenException(
                new ExceptionParameter(token, nameof(token)));
        
        var userIdDto = TokenService.ReadTokenPayload<UserIdDto>(token);
        var userDto = UserRepository.Get(userIdDto.Id).ToTransfer();
        var userIdUnameRoleDto = new UserIdUnameRoleDto 
            { Id = userDto.Id, UniqueName = userDto.UniqueName, Role = userDto.Role};
        
        refreshToken = TokenService.GenerateToken(userIdDto, TimeLiveTokens.RefreshToken);
        accessToken = TokenService.GenerateToken(userIdUnameRoleDto, TimeLiveTokens.AccessToken);
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
        if (token == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(token, nameof(token)));
        
        role = null;
        if (TokenService.ValidateToken(token) == false)
            return false;

        var dto = TokenService.ReadTokenPayload<UserIdUnameRoleDto>(token);
        if (Enum.TryParse(dto.Role, out UserRoles r))
            role = r;
        else throw new InvalidArgumentException(
                new ExceptionParameter(dto.Role, nameof(dto.Role)));
        
        return true;
    }
}