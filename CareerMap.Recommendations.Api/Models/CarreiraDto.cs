using CareerMap.Recommendations.Domain.Entities;

namespace CareerMap.Recommendations.Api.Models;

public class CarreiraDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public int Nivel { get; set; }
    public List<CompetenciaDto> CompetenciasNecessarias { get; set; } = new List<CompetenciaDto>();

    public static CarreiraDto FromEntity(Carreira entity) => new CarreiraDto
    {
        Id = entity.Id,
        Nome = entity.Nome,
        Descricao = entity.Descricao,
        Area = entity.Area,
        Nivel = entity.Nivel,
        CompetenciasNecessarias = entity.CompetenciasNecessarias.Select(CompetenciaDto.FromEntity).ToList()
    };
}

public class CompetenciaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;

    public static CompetenciaDto FromEntity(Competencia entity) => new CompetenciaDto
    {
        Id = entity.Id,
        Nome = entity.Nome,
        Tipo = entity.Tipo
    };
}
