
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using FortuneTellerService.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using FortuneTeller4.Services;

namespace FortuneTellerService.Controllers
{
    [Route("api")]
    public class FortunesController : Controller
    {
        private IFortuneRepository _fortunes;
        private IEurekaClientService _eurekaClientService;
        private ILogger<FortunesController> _logger;
        public FortunesController(IFortuneRepository fortunes, IEurekaClientService eurekaClientService, ILogger<FortunesController> logger)
        {
            _fortunes = fortunes;
            _eurekaClientService = eurekaClientService;
            _logger = logger;
        }

        // GET: api/services
        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            _logger?.LogInformation("api/services");
            return Ok(await _eurekaClientService.GetServices());
        }

        // GET: api/services/hystrix
        [HttpGet("services/hystrix")]
        public async Task<IActionResult> GetServicesWithHystrix()
        {
            _logger?.LogInformation("api/services/hystrix");
            return Ok(await _eurekaClientService.GetServicesWithHystrix());
        }

        // GET: api/fortunes
        [HttpGet("random")]
        public IEnumerable<Fortune> Get()
        {
            _logger?.LogInformation("GET api/fortunes");
            if (HttpContext.Request.Query?.Count > 0)
            {
                StringValues values;
                if (HttpContext.Request.Query.TryGetValue("Ids", out values))
                {
                    return _fortunes.GetSome(values.ToList());
                }
            }
            return _fortunes.GetAll();
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public Fortune Random()
        {
            _logger?.LogInformation("GET api/fortunes/random");
            return _fortunes.RandomFortune();
        }
    }
}
