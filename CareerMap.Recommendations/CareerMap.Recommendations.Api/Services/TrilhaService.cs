using CareerMap.Recommendations.Api.Models;
using System.Net.Http.Json;

namespace CareerMap.Recommendations.Api.Services;

public class TrilhaService : ITrilhaService
{
    private readonly HttpClient _httpClient;

    public TrilhaService(IHttpClientFactory httpClientFactory)
    {
        // Usa o nome configurado no Program.cs
        _httpClient = httpClientFactory.CreateClient("JavaApi");
    }

    public async Task<TrilhaDto> GerarTrilhaAsync(TrilhaRequestDto request)
    {
        // Endpoint da sua API Java para gerar trilhas
        const string javaApiEndpoint = "api/trilhas/gerar"; 

        try
        {
            // 1. Faz a chamada HTTP POST para a API Java
            var response = await _httpClient.PostAsJsonAsync(javaApiEndpoint, request);

            // 2. Verifica se a chamada foi bem-sucedida (Status Code 2xx)
            response.EnsureSuccessStatusCode();

            // 3. Desserializa a resposta JSON da API Java para um DTO
            var trilhaGerada = await response.Content.ReadFromJsonAsync<TrilhaDto>();

            // 4. Retorna a trilha gerada
            return trilhaGerada ?? throw new Exception("Resposta da API Java vazia ou inválida.");
        }
        catch (HttpRequestException ex)
        {
            // Trata erros de comunicação (ex: API Java fora do ar)
            throw new Exception($"Erro ao comunicar com a API Java: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Trata outros erros
            throw new Exception($"Erro na geração da trilha: {ex.Message}");
        }
    }
}
