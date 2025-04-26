using ERP.Server.Application.Features.Auth.Login;
using ERP.Server.Domain.Entities;

namespace ERP.Server.Application.Services;
public interface IJwtProvider
{
    Task<LoginCommandResponse> CreateToken(AppUser user);
}
