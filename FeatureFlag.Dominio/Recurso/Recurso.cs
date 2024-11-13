using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public sealed class Recurso : EntidadeBase
{
    public string Identificador { get; set; }
    public string Descricao { get; set; }
    
    //
    
    public List<Consumidor> Consumidores { get; set; } = [];
    public List<RecursoConsumidor> RecursoConsumidores { get; set; } = [];
}