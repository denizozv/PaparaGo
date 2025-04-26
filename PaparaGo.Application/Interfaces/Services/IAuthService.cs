using PaparaGo.Application.DTO;
using PaparaGo.DTO;

namespace PaparaGo.Application.Interfaces.Services;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto loginDto);
    Task RegisterPersonelAsync(CreatePersonelRequestDto dto);

}
