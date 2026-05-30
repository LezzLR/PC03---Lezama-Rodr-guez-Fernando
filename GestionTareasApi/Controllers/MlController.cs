using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using GestionTareasApi.Services;

namespace GestionTareasApi.Controllers
{
    [ApiController]
    [Route("api/ml")]
    public class MlController : ControllerBase
    {
        private readonly SentimentAnalysisService _sentimentAnalysisService;

        public MlController(SentimentAnalysisService sentimentAnalysisService)
        {
            _sentimentAnalysisService = sentimentAnalysisService;
        }

        [HttpPost("sentimiento")]
        public IActionResult AnalizarSentimiento([FromBody] SentimentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Comentario))
            {
                return BadRequest(new { mensaje = "El comentario es obligatorio y no puede estar vacío." });
            }

            var sentimiento = _sentimentAnalysisService.PredictSentiment(request.Comentario);

            return Ok(new
            {
                comentario = request.Comentario,
                sentimiento = sentimiento
            });
        }
    }

    public class SentimentRequest
    {
        [Required(ErrorMessage = "El comentario es obligatorio.")]
        public string Comentario { get; set; } = string.Empty;
    }
}
