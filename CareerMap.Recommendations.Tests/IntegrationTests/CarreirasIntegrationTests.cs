using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using CareerMap.Recommendations.Api.Models;
using CareerMap.Recommendations.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace CareerMap.Recommendations.Tests.IntegrationTests;

public class CarreirasIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public CarreirasIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove o DbContext de produção
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<RecommendationsDbContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                // Adiciona um DbContext em memória para testes
                services.AddDbContext<RecommendationsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Garante que o banco de dados em memória seja criado e populado
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<RecommendationsDbContext>();
                    db.Database.EnsureCreated();
                    // Opcional: Adicionar seed data específico para testes, se necessário
                }
            });
        });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetCarreiras_ReturnsSuccessAndCorrectContentType()
    {
        // Arrange
        var url = "/api/v1/Carreiras";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task GetCarreiraById_ReturnsNotFoundForInvalidId()
    {
        // Arrange
        var url = "/api/v1/Carreiras/999";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); // Status Code 404
    }

    // [Fact]
    // public async Task PostCarreira_ReturnsCreated()
    // {
    //     // Arrange
    //     var newCarreira = new CarreiraDto
    //     {
    //         Nome = "Arquiteto de Soluções",
    //         Descricao = "Projeta sistemas complexos.",
    //         Area = "Arquitetura",
    //         Nivel = 3,
    //         CompetenciasNecessarias = new List<CompetenciaDto>()
    //     };

    //     var json = JsonSerializer.Serialize(newCarreira);
    //     var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

    //     // Act
    //     var response = await _client.PostAsync("/api/v1/Carreiras", content);

    //     // Assert
    //     response.EnsureSuccessStatusCode();
    //     Assert.Equal(HttpStatusCode.Created, response.StatusCode); // Status Code 201
    // }
}
