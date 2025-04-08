using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.Extensions.Configuration;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService,IAuthService
    {
        private readonly IHttpClientFactory _httpClient;
        private string villaUrl;
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            villaUrl = configuration.GetValue<string>("VillaAPI");
        }

        public async Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = loginRequestDTO,
                Url = villaUrl + "/api/v1/Users/login"
            });
        }

        public async Task<T> RegisterAsync<T>(RegisterationRequestDTO registerationRequestDTO)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Data = registerationRequestDTO,
                Url = villaUrl + "/api/v1/Users/register"
            });
        }
    }
}
