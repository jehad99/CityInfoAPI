using CityInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesData _data;

        public CitiesController(CitiesData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }
        [HttpGet]
        public ActionResult<IEnumerable<City>> GetCities()
        {
            return Ok(_data.Cities);
        }

        [HttpGet("{id}")]
        public ActionResult<City> GetCity(int id)
        {
            var city = (_data.Cities.FirstOrDefault(i => i.Id == id));
            if (city == null) { return NotFound(); }
            return Ok(city);
        }
    }
}
