using MagicVilla.Api.Models;
using MagicVilla.Api.Models.Dto;

namespace MagicVilla.Api.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string userName);
        Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto);
        Task<LocalUser> Register(RegistrationRequestDto loginRequestDto);
    }
}
