using Microsoft.AspNetCore.Mvc;

namespace ParkYourLark.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost("space")]
        public void Post([FromBody] object request) { }
    }
}
