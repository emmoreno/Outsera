using GoldenRaspberryAwards.Entities;
using GoldenRaspberryAwards.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoldenRaspberryAwards.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntervaloPremiosController : ControllerBase
    {

        private readonly ILogger<IntervaloPremiosController> _logger;
        private readonly IMovieService _movieService;

        public IntervaloPremiosController(ILogger<IntervaloPremiosController> logger, 
                                          IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        [HttpGet(Name = "Obter Intervalo de premios")]
        public async Task<ActionResult<IntervaloPremioEntity>> Get()
        {
            try
            {
                var resultado = _movieService.GetIntervaloDePremios();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
