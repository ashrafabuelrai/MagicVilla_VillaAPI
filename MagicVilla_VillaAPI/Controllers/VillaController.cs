using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logger;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogging _logging;

        public VillaController(ILogging logging)
        {
            _logging = logging;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Villa> GetAll()
        {
            return Ok(new List<Villa>()
            {
                new Villa (){Name="a"},
                new Villa(){Name="b"}
            });
        }
        [HttpPatch("id")]
        public IActionResult Update(int id ,JsonPatchDocument<VillaDTO>patch)
        {
            //VillaDTO villa = VillaStore.villaDTOs.FirstOrDefault(v => v.Id == id);
            //patch.ApplyTo(villa, ModelState);
            if(id==0)
            {
                _logging.Log("Not founded","error");
                return BadRequest();
            }
            return Ok();
        }
    }
}
