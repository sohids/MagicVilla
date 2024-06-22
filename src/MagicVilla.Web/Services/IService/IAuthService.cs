using MagicVilla.Web.Models.Dto;

namespace MagicVilla.Web.Services.IService
{
    public interface IAuthService
    {
        Task<T?> LoginAsync<T>(LoginRequestDto objToCreate);
        Task<T?> RegistrationAsync<T>(RegistrationRequestDto objToCreate);
    }
}
