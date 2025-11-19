using Microsoft.EntityFrameworkCore;
using CareerMap.Recommendations.Domain.Entities;
using System.Text.Json;

namespace CareerMap.Recommendations.Infrastructure.Data;

public class RecommendationsDbContext : DbContext
{
    public RecommendationsDbContext(DbContextOptions<RecommendationsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Carreira> Carreiras { get; set; }
    public DbSet<Competencia> Competencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração da entidade Carreira
        modelBuilder.Entity<Carreira>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.Area).HasMaxLength(50);
            entity.Property(e => e.Nivel).IsRequired();

            // Configuração para armazenar CompetenciasNecessarias como JSON (para SQLite)
            entity.Property(e => e.CompetenciasNecessarias)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                    v => JsonSerializer.Deserialize<List<Competencia>>(v, (JsonSerializerOptions?)null) ?? new List<Competencia>()
                );
        });

        // Configuração da entidade Competencia (usada como tipo complexo/serializado, mas também como entidade para catálogo)
        modelBuilder.Entity<Competencia>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tipo).HasMaxLength(50);
        });

        // Seed inicial de dados
        var comp1 = new Competencia { Id = 1, Nome = "C#", Tipo = "Hard Skill" };
        var comp2 = new Competencia { Id = 2, Nome = ".NET Core", Tipo = "Hard Skill" };
        var comp3 = new Competencia { Id = 3, Nome = "SQL", Tipo = "Hard Skill" };
        var comp4 = new Competencia { Id = 4, Nome = "Comunicação", Tipo = "Soft Skill" };
        var comp5 = new Competencia { Id = 5, Nome = "Resolução de Problemas", Tipo = "Soft Skill" };

        modelBuilder.Entity<Competencia>().HasData(comp1, comp2, comp3, comp4, comp5);

        modelBuilder.Entity<Carreira>().HasData(
            new Carreira
            {
                Id = 1,
                Nome = "Desenvolvedor Backend .NET Júnior",
                Descricao = "Foco em APIs RESTful com .NET e Entity Framework.",
                Area = "Tecnologia",
                Nivel = 1,
                CompetenciasNecessarias = new List<Competencia> { comp1, comp2, comp3 }
            }
        );
    }
}
