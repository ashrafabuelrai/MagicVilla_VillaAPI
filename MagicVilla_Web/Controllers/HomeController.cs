using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_Web.Models;
using AutoMapper;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Utility;

namespace MagicVilla_Web.Controllers;

public class HomeController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IMapper _mapper;
    public HomeController(IMapper mapper, IVillaService villaService)
    {
        _mapper = mapper;
        _villaService = villaService;
    }
    public async Task<IActionResult> Index()
    {
        List<VillaDTO> list = new List<VillaDTO>();
        var respone = await _villaService.GetAllAsync<ApiResponse>(HttpContext.Session.GetString(SD.SessionToken));
        if (respone != null && respone.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(respone.Result));
        }
        return View(list);
    }

}
