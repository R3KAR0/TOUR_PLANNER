using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Microsoft.AspNetCore.Mvc;
using Serilog;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAUER_SWEN2_TOUR_PLANNER.REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourLogController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    List<string> res = new();
                    var tours = unit.TourLogRepository().GetAll();
                    foreach (var t in tours)
                    {
                        res.Add(System.Text.Json.JsonSerializer.Serialize(t));
                    }

                    return res;
                }
            }
            catch (Exception)
            {
                Log.Error("Failed to get TourLog in ToursController!");
                return new List<string>();
            }
        }

        // GET api/<ToursController>/5
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    return System.Text.Json.JsonSerializer.Serialize(unit.TourLogRepository().GetById(id));
                }
            }
            catch (Exception)
            {
                Log.Error($"Failed to get TourLog with the id: {id}!");
                return "";

            }
        }

        // POST api/<ToursController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    unit.TourLogRepository().Add(System.Text.Json.JsonSerializer.Deserialize<TourLog>(value));

                    return;
                }
            }
            catch (Exception)
            {
                Log.Error($"Failed to add TourLog! Received: {value}");
            }
        }

        // PUT api/<ToursController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] string value)
        {

        }

        // DELETE api/<ToursController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            try
            {
                using (UnitOfWork unit = new())
                {
                    unit.TourLogRepository().Delete(id);
                }
            }
            catch (Exception)
            {
                Log.Error($"Failed to delete TourLog with id: {id}!");

            }
        }
    }
}
