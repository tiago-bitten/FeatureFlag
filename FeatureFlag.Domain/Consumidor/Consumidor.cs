using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public sealed class Consumidor : EntidadeBase
{
    public string Identificador { get; set; }
    public string Descricao { get; set; }
    
    //

    public List<Recurso> Recursos { get; set; } = [];
    public List<RecursoConsumidor> RecursoConsumidores { get; set; } = [];
}