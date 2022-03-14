using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace RestApp.Controllers
{
    public class Test
    {
        public float Value { get; set; }
    }

    public abstract class IncumplimientoDto
    {
        public Guid TipoIncumplimientoId { get; set; }
        public string NumeroFactura { get; set; }
        public decimal Importe { get; set; }
        public bool Aplica { get; set; }
        public string Observaciones { get; set; }
    }

    public class ChangeExpedienteOficioIncumplimientoDto : IncumplimientoDto
    {
        public Guid Id { get; set; }
        public Guid ExpedienteId { get; set; }

        public ChangeExpedienteOficioIncumplimientoDto() { }
    }

    [ApiController]
    [Route("")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello!";
        }

        [HttpPut]
        public string Put([FromBody] ChangeExpedienteOficioIncumplimientoDto test)
        {
            string info = $"Importe {test.Importe}";
            _logger.LogInformation(info);
            return info;
        }
    }
}
