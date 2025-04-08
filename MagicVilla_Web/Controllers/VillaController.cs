using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IMapper mapper, IVillaService villaService)
        {
            _mapper = mapper;
            _villaService = villaService;
        }
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new List<VillaDTO>();
            var respone = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (respone != null && respone.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respone.Result));
            }
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVilla()
        {

            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var respone = await _villaService.CreateAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),model);
                if (respone != null && respone.IsSuccess)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Something goes wrong";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVilla(int id)
        {
            var respone = await _villaService.GetAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),id);
            if (respone != null && respone.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(respone.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var respone = await _villaService.UpdateAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),model);
                if (respone != null && respone.IsSuccess)
                {
                    TempData["success"] = "Villa updated successfully";
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Something goes wrong";
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var respone = await _villaService.GetAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),id);
            if (respone != null && respone.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(respone.Result));
                return View(model);
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {

            var respone = await _villaService.DeleteAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken),model.Id);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Something goes wrong";
            return View(model);
        }
    }
}
