using CareerMap.Recommendations.Api.Models;

namespace CareerMap.Recommendations.Api.Services;

public interface ITrilhaService
{
    /// <summary>
    /// Simula ou executa a chamada para a API Java para gerar uma trilha com IA.
    /// </summary>
    /// <param name="request">Dados do usuário para a geração da trilha.</param>
    /// <returns>A TrilhaDto gerada.</returns>
    Task<TrilhaDto> GerarTrilhaAsync(TrilhaRequestDto request);
}

public record TrilhaRequestDto(string Perfil, List<string> CompetenciasDesejadas);

public record TrilhaDto(string Nome, string Descricao, List<string> Passos);
