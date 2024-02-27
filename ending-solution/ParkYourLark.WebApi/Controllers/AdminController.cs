using Microsoft.AspNetCore.Mvc;
using ParkYourLark.WebApi.Data;
using System.Linq;

namespace ParkYourLark.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRequestParser _requestParser;
        private readonly IDataAccess _dataAccess;

        public AdminController(IRequestParser requestParser, IDataAccess dataAccess)
        {
            _requestParser = requestParser;
            _dataAccess = dataAccess;
        }

        [HttpPost("space")]
        public void Post([FromBody] object request)
        {
            var levelSpace = _requestParser.Parse(request);
            var levels = _dataAccess.Get<Level>();

            if (!levels.Contains(levelSpace.Level))
            {
                _dataAccess.Add(levelSpace.Level);
            }
            _dataAccess.Add(levelSpace);
        }
    }
}
