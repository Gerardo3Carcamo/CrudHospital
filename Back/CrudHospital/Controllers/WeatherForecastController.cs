using Microsoft.AspNetCore.Mvc;

namespace CrudHospital.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public ActionResult test()
        {
            return Ok(new {data = "Hola", error = false, msg = "Ok", apiName = "Test"});
        }
    }
}