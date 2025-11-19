using Microsoft.AspNetCore.Mvc;
using CareerMap.Recommendations.Api.Services;
using CareerMap.Recommendations.Api.Models;

namespace CareerMap.Recommendations.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class TrilhasController : ControllerBase
{
    private readonly ITrilhaService _trilhaService;

    public TrilhasController(ITrilhaService trilhaService)
    {
        _trilhaService = trilhaService;
    }

    // POST: api/v1/Trilhas/gerar
    /// <summary>
    /// Gera uma trilha personalizada com IA, comunicando-se com a API Java.
    /// </summary>
    /// <param name="request">Dados do usuário para a geração da trilha.</param>
    /// <returns>A TrilhaDto gerada.</returns>
    [HttpPost("gerar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<TrilhaDto>> GerarTrilha([FromBody] TrilhaRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var trilha = await _trilhaService.GerarTrilhaAsync(request);
            
            // Aqui você adicionaria a lógica para salvar o histórico da trilha no seu DB .NET
            // Ex: _context.Trilhas.Add(Trilha.FromDto(trilha)); await _context.SaveChangesAsync();

            return Ok(trilha);
        }
        catch (Exception ex)
        {
            // Retorna 503 se a API Java estiver fora do ar ou inacessível
            if (ex.Message.Contains("API Java"))
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = "Serviço de Geração de Trilha (API Java) indisponível.", details = ex.Message });
            }
            
            return BadRequest(new { message = "Erro ao processar a requisição de trilha.", details = ex.Message });
        }
    }
}
