using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(string token,int id);
        Task<T> CreateAsync<T>(string token,VillaNumberCreateDTO dto);
        Task<T> UpdateAsync<T>(string token,VillaNumberUpdateDTO dto);
        Task<T> DeleteAsync<T>(string token,int id);
    }
}
