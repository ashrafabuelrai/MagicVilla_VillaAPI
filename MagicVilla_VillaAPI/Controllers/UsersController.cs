
using Asp.Versioning;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApiResponse _apiResponse;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _apiResponse = new ApiResponse();
        }
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse>> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var loginRespone = await _unitOfWork.User.Login(loginDTO);
            if(loginRespone.User==null || string.IsNullOrEmpty(loginRespone.Token))
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { "User or password is incorrect" };
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = loginRespone;
            return Ok(_apiResponse);
        }
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterationRequestDTO registerDTO)
        {
            bool ifUserNameUnique =  _unitOfWork.User.IsUniqueUser(registerDTO.UserName);
            if(!ifUserNameUnique)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { "Username already exists" };
                return BadRequest(_apiResponse);
            }
            var user = await _unitOfWork.User.Register(registerDTO);
            if(user==null)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = new List<string> { "Error while registering..." };
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }
    }
}
