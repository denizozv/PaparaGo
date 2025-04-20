using PaparaGo.Application.DTO;

namespace PaparaGo.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
}
