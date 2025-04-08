using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(string token,int id);
        Task<T> CreateAsync<T>(string token,VillaCreateDTO dto);
        Task<T> UpdateAsync<T>(string token,VillaUpdateDTO dto);
        Task<T> DeleteAsync<T>(string token,int id);
    }
}
