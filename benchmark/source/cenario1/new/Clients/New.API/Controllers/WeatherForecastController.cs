using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductModule.Queries;
using SharedKernel;
using SharedKernel.Distribuited;

namespace New.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOutputTransport _queryDispatcher;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOutputTransport queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _queryDispatcher.Handle<IEnumerable<Product>, GetAllProducts>(new GetAllProducts());
        }
    }
}