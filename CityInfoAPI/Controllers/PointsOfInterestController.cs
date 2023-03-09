using CityInfoAPI.DTOs;
using CityInfoAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterest>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointsOfInterest = city.PointsOfInterest.ToList();

            return Ok(pointsOfInterest);
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterest> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesData.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == pointOfInterestId);
            if (pointOfInterest == null)
            {
                return NotFound();
            }
            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterest> CreatePointOfInterest(int cityId, AddPointOfInterest pointOfInterest)
        {
            var city = CitiesData.Current.Cities.FirstOrDefault(i => i.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var maxId = CitiesData.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(i => i.Id);
            var createdPointOfInterest = new PointOfInterest()
            {
                Id = ++maxId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };
            city.PointsOfInterest.Add(createdPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = createdPointOfInterest.Id
                },
               createdPointOfInterest);
        }
    }
}
