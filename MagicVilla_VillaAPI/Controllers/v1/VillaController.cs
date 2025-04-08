
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
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
  
    public class VillaController : ControllerBase
    {
        protected ApiResponse _response { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VillaController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _response = new ApiResponse();
        }
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> GetAll([FromQuery(Name ="Occupancy Filtter")]int occ,int PageSize=0,int PageNumber=1)
        {
            try
            {
                IEnumerable<Villa> villas;
                if (occ > 0)
                {
                    villas= _unitOfWork.Villa.GetAll(v=>v.Occupancy==occ,pageSize:PageSize,pageNumber:PageNumber);
                }
                else
                {
                    villas = _unitOfWork.Villa.GetAll(pageSize:PageSize,pageNumber: PageNumber);
                }
                if (villas == null || villas.Count() == 0)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessage = new List<string>() { "The villa not found!" };
                    return NotFound(_response);
                }
                Pagination pagination = new Pagination() { PageNumber = PageNumber, PageSize = PageSize };
                Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(pagination));//JsonConvert.SerializeObject(pagination));
                _response.Result = _mapper.Map<IEnumerable<VillaDTO>>(villas);
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
        [HttpGet("{id:int}",Name = "GetVilla")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villa = _unitOfWork.Villa.Get(v => v.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = new List<string>() { "The villa not found!" };
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<VillaDTO>(villa);
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
        [HttpPost(Name = "CreateVilla")]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> CreateVilla([FromBody] VillaCreateDTO villaCreateDTO)
        {
            try
            {
                if (_unitOfWork.Villa.Get(v => v.Name == villaCreateDTO.Name) != null)
                {
                    _response.ErrorMessage= new List<string> {"The villa already exists"};
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                if (villaCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                Villa villa = _mapper.Map<Villa>(villaCreateDTO);
                _unitOfWork.Villa.Add(villa);
                _unitOfWork.Save();
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpDelete("{id:int}",Name = "DeleteVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ApiResponse> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                var villa = _unitOfWork.Villa.Get(v => v.Id == id);
                if (villa == null)
                {
                   _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ErrorMessage = new List<string>() { "The villa not found!" };
                    _response.IsSuccess = false;
                    return NotFound(_response);
                }
                _unitOfWork.Villa.Remove(villa);
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
        [HttpPut("{id:int}",Name = "UpdateVilla")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ApiResponse> UpdateVilla(int id,[FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                if (villaUpdateDTO == null || id != villaUpdateDTO.Id)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                Villa villa = _mapper.Map<Villa>(villaUpdateDTO);
                _unitOfWork.Villa.Update(villa);
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
        [HttpPatch("{id:int}",Name = "UpdatePartailVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartailVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id ==0)
            {
                return BadRequest();
            }
            var villa = _unitOfWork.Villa.Get(v => v.Id == id);
            VillaUpdateDTO villaUpdateDTO = _mapper.Map<VillaUpdateDTO>(villa);
            if (villa==null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaUpdateDTO, ModelState);

            villa = _mapper.Map<Villa>(villaUpdateDTO);
            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Save();
            if (!ModelState.IsValid) return BadRequest();
            return NoContent();
        }
    }
}
