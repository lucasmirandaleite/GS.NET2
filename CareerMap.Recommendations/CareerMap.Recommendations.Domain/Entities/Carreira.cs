namespace CareerMap.Recommendations.Domain.Entities;

public class Carreira
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public int Nivel { get; set; } // Ex: 1 (Junior), 2 (Pleno), 3 (Senior)
    public List<Competencia> CompetenciasNecessarias { get; set; } = new List<Competencia>();
}

public class Competencia
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // Ex: "Hard Skill", "Soft Skill"
}
