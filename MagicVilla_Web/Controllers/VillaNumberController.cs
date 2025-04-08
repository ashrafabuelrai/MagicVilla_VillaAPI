using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using MagicVilla_Utility;
namespace MagicVillaNumber_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _VillaNumberService;
        private readonly IVillaService _VillaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IMapper mapper, IVillaNumberService VillaNumberService,IVillaService VillaService)
        {
            _mapper = mapper;
            _VillaNumberService = VillaNumberService;
            _VillaService = VillaService;
        }
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new List<VillaNumberDTO>();
            var respone = await _VillaNumberService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(respone.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM VillaNumberVM = new VillaNumberCreateVM();

            var respone = await _VillaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                VillaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respone.Result))
                    .Select(
                       u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       }
                    );
            }
            return View(VillaNumberVM);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var Respone = await _VillaNumberService.CreateAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),model.VillaNumber);
                if (Respone != null && Respone.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if(Respone.ErrorMessage.Count!=0)
                    {
                        ModelState.AddModelError("ErrorMessages", Respone.ErrorMessage.FirstOrDefault());
                    }
                }    
            }

            var respone = await _VillaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respone.Result))
                    .Select(
                       u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       }
                    );
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNumberUpdateVM = new();
            var respone = await _VillaNumberService.GetAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),villaNo);
            if (respone != null && respone.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(respone.Result));
                villaNumberUpdateVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);
            }

            respone = await _VillaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respone.Result))
                    .Select(
                       u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       }
                    );
                return View(villaNumberUpdateVM);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var Respone = await _VillaNumberService.UpdateAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),model.VillaNumber);
                if (Respone != null && Respone.IsSuccess)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    if (Respone.ErrorMessage.Count != 0)
                    {
                        ModelState.AddModelError("ErrorMessages", Respone.ErrorMessage.FirstOrDefault());
                    }
                }
            }

            var respone = await _VillaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respone.Result))
                    .Select(
                       u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       }
                    );
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberDeleteVM villaNumberDeleteVM = new();
            var respone = await _VillaNumberService.GetAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),villaNo);
            if (respone != null && respone.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(respone.Result));
                villaNumberDeleteVM.VillaNumber = model;
            }

            respone = await _VillaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                villaNumberDeleteVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respone.Result))
                    .Select(
                       u => new SelectListItem
                       {
                           Text = u.Name,
                           Value = u.Id.ToString()
                       }
                    );
                return View(villaNumberDeleteVM);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {

            var respone = await _VillaNumberService.DeleteAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),model.VillaNumber.VillaNo);
            if (respone != null && respone.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }

            return View(model);
        }
    }
}
