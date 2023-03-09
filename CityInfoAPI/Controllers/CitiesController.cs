using CityInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<City> GetCities()
        {
            return new JsonResult (CitiesData.Current.Cities.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<City> GetCity(int id)
        {
            return new JsonResult(CitiesData.Current.Cities.FirstOrDefault(i => i.Id == id));
        }
    }
}
