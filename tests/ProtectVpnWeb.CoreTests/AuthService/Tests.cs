using NUnit.Framework.Internal;
using ProtectVpnWeb.Core.Dto.Auth;
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

    public Tests()
    {
        _userRepository = new MockUserRepository();
        _tokenRepository = new MockTokenRepository();
        _tokenService = new MockTokenService();
        _hashService = new MockHashService();
        var times = new TimeLiveTokensDto
        {
            RefreshAuthToken = TimeSpan.FromSeconds(30),
            RefreshToken = TimeSpan.FromDays(1),
            AccessToken = TimeSpan.FromMinutes(15)
        };
        _service = new AuthService<
                MockUserRepository, MockTokenRepository, MockTokenService, MockHashService>
            (_userRepository, _tokenRepository, _tokenService, _hashService, times);
    }

    [Test]
    public async Task AuthUser_Success()
    {
        
    }
}