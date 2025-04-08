using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Identity.Data;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IUserRepository:IRepository<ApplicationUser>
    {
        bool IsUniqueUser(string username);
        Task<LoginResponsDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);

    }
}
