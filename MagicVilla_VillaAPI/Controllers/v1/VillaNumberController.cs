
using Asp.Versioning;
using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaNumberController : ControllerBase
    {
        protected ApiResponse _response { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VillaNumberController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> GetAll()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProp:"Villa");
                if(villaNumbers==null||villaNumbers.Count()==0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessage = new List<string>() { "The villa not found!" };
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<IEnumerable<VillaNumberDTO>>(villaNumbers);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        
        [HttpGet("{villNo:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> GetVillaNumber(int villNo)
        {
            try
            {
                if (villNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaNumber = _unitOfWork.VillaNumber.Get(v => v.VillaNo == villNo);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = new List<string>() { "The villa not found!" };
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPost(Name = "CreateVillaNumber")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {
                if (_unitOfWork.VillaNumber.Get(v => v.VillaNo == villaNumberCreateDTO.VillaNo) != null)
                {
                    _response.ErrorMessage= new List<string> {"Villa number already exists"};
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                if (_unitOfWork.Villa.Get(v => v.Id == villaNumberCreateDTO.VillaId) == null)
                {
                    _response.ErrorMessage = new List<string> { "The Villa not exists" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                if (villaNumberCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberCreateDTO);
                _unitOfWork.VillaNumber.Add(villaNumber);
                _unitOfWork.Save();
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { villNo = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete("{villNo:int}", Name = "DeleteVillaNumber")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> DeleteVillaNumber(int villNo)
        {
            try
            {
                if (villNo == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villaNumber = _unitOfWork.VillaNumber.Get(v => v.VillaNo == villNo);
                if (villaNumber == null)
                {
                   _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = new List<string>() { "The villa not found!" };
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _unitOfWork.VillaNumber.Remove(villaNumber);
                _unitOfWork.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPut("{villNo:int}", Name = "UpdateVillaNumber")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ApiResponse> UpdateVillaNumber(int villNo, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            try
            {
                if (villaNumberUpdateDTO == null || villNo != villaNumberUpdateDTO.VillaNo)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                if (_unitOfWork.Villa.Get(v => v.Id == villaNumberUpdateDTO.VillaId) == null)
                {
                    _response.ErrorMessage = new List<string> { "The Villa not exists" };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
                _unitOfWork.VillaNumber.Update(villaNumber);
                _unitOfWork.Save();
                _response.StatusCode = HttpStatusCode.NoContent;
               
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPatch("{villNo:int}", Name = "UpdatePartailVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartailVillaNumber(int villNo, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || villNo == 0)
            {
                return BadRequest();
            }
            var villaNumber = _unitOfWork.VillaNumber.Get(v => v.VillaNo == villNo);
            VillaNumberUpdateDTO villaNumberUpdateDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);
            if (villaNumber==null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaNumberUpdateDTO, ModelState);

            villaNumber = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);
            _unitOfWork.VillaNumber.Update(villaNumber);
            _unitOfWork.Save();
            if (!ModelState.IsValid) return BadRequest();
            return NoContent();
        }
    }
}
