using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CareerMap.Recommendations.Infrastructure.Data;
using CareerMap.Recommendations.Api.Models;
using CareerMap.Recommendations.Domain.Entities;

namespace CareerMap.Recommendations.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class CarreirasController : ControllerBase
{
    private readonly RecommendationsDbContext _context;

    public CarreirasController(RecommendationsDbContext context)
    {
        _context = context;
    }

    // GET: api/v1/Carreiras?page=1&pageSize=10
    /// <summary>
    /// Retorna uma lista paginada de carreiras.
    /// </summary>
    /// <param name="page">Número da página (padrão: 1).</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10).</param>
    /// <returns>Uma lista paginada de CarreiraDto com links HATEOAS.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarreiraDto>>> GetCarreiras(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var totalCount = await _context.Carreiras.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var carreiras = await _context.Carreiras
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = carreiras.Select(c => CarreiraDto.FromEntity(c)).ToList();

        // Implementação HATEOAS (simplificada)
        var result = new
        {
            Metadata = new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
                TotalPages = totalPages
            },
            Links = new
            {
                Self = Url.Action(nameof(GetCarreiras), new { page, pageSize }),
                Next = page < totalPages ? Url.Action(nameof(GetCarreiras), new { page = page + 1, pageSize }) : null,
                Prev = page > 1 ? Url.Action(nameof(GetCarreiras), new { page = page - 1, pageSize }) : null
            },
            Items = dtos.Select(AddHateoasLinks)
        };

        return Ok(result);
    }

    // GET: api/v1/Carreiras/5
    /// <summary>
    /// Retorna uma carreira específica.
    /// </summary>
    /// <param name="id">ID da carreira.</param>
    /// <returns>A CarreiraDto com links HATEOAS.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarreiraDto>> GetCarreira(int id)
    {
        var carreira = await _context.Carreiras.FindAsync(id);

        if (carreira == null)
        {
            return NotFound(); // Status Code 404
        }

        return Ok(AddHateoasLinks(CarreiraDto.FromEntity(carreira)));
    }

    // POST: api/v1/Carreiras
    /// <summary>
    /// Cria uma nova carreira.
    /// </summary>
    /// <param name="carreiraDto">Dados da nova carreira.</param>
    /// <returns>A CarreiraDto criada com links HATEOAS.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarreiraDto>> PostCarreira(CarreiraDto carreiraDto)
    {
        var carreira = new Carreira
        {
            Nome = carreiraDto.Nome,
            Descricao = carreiraDto.Descricao,
            Area = carreiraDto.Area,
            Nivel = carreiraDto.Nivel,
            // Simplificação: CompetenciasNecessarias não são tratadas em profundidade no POST
            // Em um cenário real, Competencias seriam entidades separadas e relacionadas
            CompetenciasNecessarias = carreiraDto.CompetenciasNecessarias.Select(c => new Competencia { Nome = c.Nome, Tipo = c.Tipo }).ToList()
        };

        _context.Carreiras.Add(carreira);
        await _context.SaveChangesAsync();

        var createdDto = CarreiraDto.FromEntity(carreira);

        return CreatedAtAction(nameof(GetCarreira), new { id = createdDto.Id }, AddHateoasLinks(createdDto)); // Status Code 201
    }

    // PUT: api/v1/Carreiras/5
    /// <summary>
    /// Atualiza uma carreira existente.
    /// </summary>
    /// <param name="id">ID da carreira a ser atualizada.</param>
    /// <param name="carreiraDto">Dados atualizados da carreira.</param>
    /// <returns>NoContent (204) ou NotFound (404).</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCarreira(int id, CarreiraDto carreiraDto)
    {
        if (id != carreiraDto.Id)
        {
            return BadRequest(); // Status Code 400
        }

        var carreira = await _context.Carreiras.FindAsync(id);
        if (carreira == null)
        {
            return NotFound(); // Status Code 404
        }

        carreira.Nome = carreiraDto.Nome;
        carreira.Descricao = carreiraDto.Descricao;
        carreira.Area = carreiraDto.Area;
        carreira.Nivel = carreiraDto.Nivel;
        // Lógica de atualização de CompetenciasNecessarias omitida para simplificação

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!CarreiraExists(id))
        {
            return NotFound();
        }

        return NoContent(); // Status Code 204
    }

    // DELETE: api/v1/Carreiras/5
    /// <summary>
    /// Exclui uma carreira.
    /// </summary>
    /// <param name="id">ID da carreira a ser excluída.</param>
    /// <returns>NoContent (204) ou NotFound (404).</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCarreira(int id)
    {
        var carreira = await _context.Carreiras.FindAsync(id);
        if (carreira == null)
        {
            return NotFound(); // Status Code 404
        }

        _context.Carreiras.Remove(carreira);
        await _context.SaveChangesAsync();

        return NoContent(); // Status Code 204
    }

    private bool CarreiraExists(int id)
    {
        return _context.Carreiras.Any(e => e.Id == id);
    }

    // Helper para adicionar links HATEOAS a um DTO
    private object AddHateoasLinks(CarreiraDto dto)
    {
        return new
        {
            dto.Id,
            dto.Nome,
            dto.Descricao,
            dto.Area,
            dto.Nivel,
            dto.CompetenciasNecessarias,
            Links = new
            {
                Self = Url.Action(nameof(GetCarreira), new { id = dto.Id }),
                Update = Url.Action(nameof(PutCarreira), new { id = dto.Id }),
                Delete = Url.Action(nameof(DeleteCarreira), new { id = dto.Id })
            }
        };
    }
}
