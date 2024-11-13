using FeatureFlag.Domain;
using FeatureFlag.Repositorio.Infra;

namespace FeatureFlag.Repositorio;

public class RepMemoRecursoConsumidor : RepMemoBase<RecursoConsumidor>, IRepRecursoConsumidor
{
    public Task<RecursoConsumidor?> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorConsumidor)
    {
        var recursosConsumidor = Items.FirstOrDefault(x => x.Recurso.Descricao == descricaoRecurso && x.Consumidor.Identificador == identificadorConsumidor);

        return Task.FromResult(recursosConsumidor);
    }

    public IQueryable<RecursoConsumidor> RecuperarPorConsumidor(string identificadorConsumidor)
    {
        var recursoConsumidores = Items.Where(x => x.Consumidor.Identificador == identificadorConsumidor);
        
        return recursoConsumidores.AsQueryable();
    }
}