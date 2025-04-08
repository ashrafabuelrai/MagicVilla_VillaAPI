using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory _httpClient;
        private string villaUrl;
        public VillaService(IHttpClientFactory httpClient,IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("VillaAPI");
        }

        public async Task<T> CreateAsync<T>(string token,VillaCreateDTO dto)
        {
            return await SendAsync<T>(new ApiRequest()
            { 
                ApiType = ApiType.POST,
                Data = dto,
                Url = villaUrl +"/api/v1/Villa",
                Token = token
            });
        }

        public async Task<T> DeleteAsync<T>(string token,int id)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.DELETE,
                Url = villaUrl + "/api/v1/Villa/"+id,
                Token = token
            });
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = villaUrl + "/api/v1/Villa/",
                Token = token
            });
        }

        public async Task<T> GetAsync<T>(string token,int id)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = villaUrl + "/api/v1/Villa/" + id,
                Token = token
            });
        }

        public async Task<T> UpdateAsync<T>(string token,VillaUpdateDTO dto)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/v1/Villa/"+dto.Id,
                Token=token
            });
        }
    }
}
