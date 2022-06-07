using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using Microsoft.AspNetCore.Mvc;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LAUER_SWEN2_TOUR_PLANNER.REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        // GET: api/<ToursController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                using(UnitOfWork unit = new())
                {
                    List<string> res = new();
                    var tours = unit.TourRepository().GetAll();
                    foreach(var t in tours)
                    {
                        res.Add(System.Text.Json.JsonSerializer.Serialize(t));
                    }

                    return res;
                }
            }
            catch (Exception)
            {
                Log.Error("Failed to get Tours in ToursController!");
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
                    return System.Text.Json.JsonSerializer.Serialize(unit.TourRepository().GetById(id));
                }
            }
            catch (Exception)
            {
                Log.Error($"Failed to get Tour with the id: {id}!");
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
                    unit.TourRepository().Add(System.Text.Json.JsonSerializer.Deserialize<Tour>(value));

                    return;
                }
            }
            catch (Exception)
            {
                Log.Error($"Failed to add Tour! Received: {value}");
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
                    unit.TourRepository().Delete(id);
                }
            }
            catch (Exception)
            {
                Log.Error($"Failed to delete Tour with id: {id}!");

            }
        }
    }
}
