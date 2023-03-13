using CityInfoAPI.DTOs;
using CityInfoAPI.Models;
using CityInfoAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/pointsOfInterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly CitiesData _data;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, CitiesData data)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _data = data ?? throw new ArgumentNullException(nameof(data));
        }
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterest>> GetPointsOfInterest(int cityId)
        {
            try
            {
                //throw new Exception("Error happened");
                var city = _data.Cities.FirstOrDefault(c => c.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing pointOfInterest");
                    return NotFound();
                }
                return Ok(city.PointsOfInterest);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest with city id {cityId}", ex);
                return StatusCode(500, "A problem happened while handling the request");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterest> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _data.Cities.FirstOrDefault(c => c.Id == cityId);
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
            var city = _data.Cities.FirstOrDefault(i => i.Id == cityId);
            if (city == null) return NotFound();

            var maxId = _data.Cities.SelectMany(c => c.PointsOfInterest).Max(i => i.Id);
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

        [HttpPut("{pointOfInterestId}")]
        public ActionResult<AddPointOfInterest> UpdatePointOfInterest(int cityId, int pointOfInterestId, AddPointOfInterest interest)
        {
            var city = _data.Cities.FirstOrDefault(i => i.Id == cityId);
            if (city == null) return NotFound();


            var pointOfInterestStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestStore == null) return NotFound();

            pointOfInterestStore.Name = interest.Name;
            pointOfInterestStore.Description = interest.Description;

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult<AddPointOfInterest> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<AddPointOfInterest> patchDoc)
        {
            var city = _data.Cities.FirstOrDefault(i => i.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterestStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestStore == null) return NotFound();

            var patchedInterest = new AddPointOfInterest()
            {
                Name = pointOfInterestStore.Name,
                Description = pointOfInterestStore.Description,
            };

            patchDoc.ApplyTo(patchedInterest, ModelState);
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!TryValidateModel(patchedInterest)) return BadRequest(ModelState);  

            pointOfInterestStore.Name = patchedInterest.Name;
            pointOfInterestStore.Description = patchedInterest.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _data.Cities.FirstOrDefault(i => i.Id == cityId);
            if (city == null) return NotFound();

            var pointOfInterestStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            if (pointOfInterestStore == null) return NotFound();

            city.PointsOfInterest.Remove(pointOfInterestStore);
            _mailService.Send("Delete Point Of Interest", $"Point Of interest with id {pointOfInterestId} was deleted");
            return NoContent();
        }
    }
}
    