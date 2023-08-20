using ProtectVpnWeb.Core.Dto;
using ProtectVpnWeb.Core.Dto.Auth;
using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Services.Implementations;
using ProtectVpnWeb.CoreTests.UserService;

namespace ProtectVpnWeb.CoreTests.AuthService;

public sealed class Tests
{
    private readonly MockUserRepository _userRepository;
    private readonly MockTokenRepository _tokenRepository;
    private readonly MockTokenService _tokenService;
    private readonly MockHashService _hashService;
    private readonly AuthService<
        MockUserRepository, MockTokenRepository, MockTokenService, MockHashService> _service;

    private readonly string _fakeSol = "//hashed";
    
    private readonly User[] _fakeUsers =
    {
        new(0, "user1", "pwd1//hashed"),
        new(1, "user2", "pwd2//hashed"),
        new(2, "user3", "pwd3//hashed"),
        new(3, "user4", "pwd4//hashed"),
        new(4, "user5", "pwd5//hashed")
    };

    private readonly UserIdDto[] _fakeIdUsers = 
        { new() { Id = 2 }, new() { Id = 3 }, new() { Id = 4 } };

    private readonly string[] _fakeTokens =
    {
        "token3", "token4", "token5"
    };
    
    private readonly TimeLiveTokensDto _fakeTimes = new()
    {
        RefreshAuthToken = TimeSpan.FromSeconds(30),
        RefreshToken = TimeSpan.FromDays(1),
        AccessToken = TimeSpan.FromMinutes(15)
    };
    
    public Tests()
    {
        _userRepository = new MockUserRepository();
        _tokenRepository = new MockTokenRepository();
        _tokenService = new MockTokenService();
        _hashService = new MockHashService(_fakeSol);
        _service = new AuthService<
                MockUserRepository, MockTokenRepository, MockTokenService, MockHashService>
            (_userRepository, _tokenRepository, _tokenService, _hashService, _fakeTimes);
    }

    [Test]
    public async Task AuthUser_Success()
    {
        _userRepository.FakeInit(_fakeUsers);
        _tokenRepository.FakeInit(_fakeTokens);
        _tokenService.FakeInit(_fakeTokens, _fakeIdUsers);
        var authUser = new AuthUserDto{ UserName = "user1", Password = "pwd1" };

        var refresh = _service.AuthUser(authUser);
        
        Assert.Multiple(() =>
        {
            Assert.That(_tokenService.ValidateToken(refresh), Is.True);
            Assert.That(_tokenService.ReadTokenPayload<UserIdDto>(refresh).Id, Is.EqualTo(0));
            Assert.That(_tokenRepository.TokenExists(refresh), Is.True);
        });
    }

    [Test]
    public async Task RegisterUser_Success()
    {
        _userRepository.FakeInit(_fakeUsers);
        _tokenRepository.FakeInit(_fakeTokens);
        _tokenService.FakeInit(_fakeTokens, _fakeIdUsers);
        var regUser = new AuthUserDto{ UserName = "user6", Password = "pwd6" };

        _service.RegisterUser(regUser);

        var user = _userRepository.GetByUniqueName(regUser.UserName);
        Assert.Multiple(() =>
        {
            Assert.That(user.HashPassword, Is.EqualTo(_hashService.GetHash(regUser.Password)));
            Assert.That(user.Role, Is.EqualTo(UserRoles.User));
        });
    }
    
    [Test]
    public async Task ChangePassword_Success()
    {
        _userRepository.FakeInit(_fakeUsers);
        _tokenRepository.FakeInit(_fakeTokens);
        _tokenService.FakeInit(_fakeTokens, _fakeIdUsers);
        var changePwd = new ChangePwdDto
            { UserName = "user2", Password = "pwd2", NewPassword = "0000" };

        _service.ChangePassword(changePwd);

        var user = _userRepository.GetByUniqueName(changePwd.UserName);
        Assert.That(user.HashPassword, Is.EqualTo(_hashService.GetHash(changePwd.NewPassword)));
    }

    [Test]
    public async Task GetTokens_Success()
    {
        _userRepository.FakeInit(_fakeUsers);
        _tokenRepository.FakeInit(_fakeTokens);
        _tokenService.FakeInit(_fakeTokens, _fakeIdUsers);
        const string token = "token3";
        var user = new UserIdUnameRoleDto
            { Id = 2, UniqueName = "user3", Role = UserRoles.User.ToString() };
        
        _service.GetTokensByRefreshToken(token, 
            out var refreshToken, out var accessToken);
        
        Assert.Multiple(() =>
        {
            Assert.That(_tokenRepository.TokenExists(token), Is.False);
            Assert.That(_tokenService.ValidateToken(refreshToken), Is.True);
            Assert.That(_tokenService.ReadTokenPayload<UserIdDto>(refreshToken)
                .AreEqual(user), Is.True);
            Assert.That(_tokenService.ValidateToken(accessToken), Is.True);
            Assert.That(
                _tokenService.ReadTokenPayload<UserIdUnameRoleDto>(accessToken)
                .AreEqual(user), Is.True);
        });
    }

    [Test]
    public async Task RemoveRefreshToken_Success()
    {
        _userRepository.FakeInit(_fakeUsers);
        _tokenRepository.FakeInit(_fakeTokens);
        _tokenService.FakeInit(_fakeTokens, _fakeIdUsers);
        var token = "token4";
        
       _service.RemoveRefreshToken(token);
        
        Assert.That(_tokenRepository.TokenExists(token), Is.False);
    }

    [Test]
    public async Task ValidateAccessToken_Success()
    {
        _userRepository.FakeInit(_fakeUsers);
        _tokenRepository.FakeInit(_fakeTokens);
        _tokenService.FakeInit(_fakeTokens, _fakeIdUsers);
        const string token = "token5";
        const string fakeToken = "fakeToken";
        _service.GetTokensByRefreshToken(token,out _, out var accessToken);

        var positiveValidate = _service.ValidateAccessToken(accessToken, out var role);
        var negativeValidate = _service.ValidateAccessToken(fakeToken, out var nullRole);
        
        Assert.Multiple(() =>
        {
            Assert.That(positiveValidate, Is.True);
            Assert.That(role, Is.Not.Null);
            Assert.That(negativeValidate, Is.False);
            Assert.That(nullRole, Is.Null);
        });
    }
}