using FeatureFlag.Domain;
using FeatureFlag.Dominio.RecursoConsumidor.Dtos;
using FeatureFlag.Repositorio.Infra;

namespace FeatureFlag.Repositorio;

public class RepMemoRecursoConsumidor : RepMemoBase<RecursoConsumidor>, IRepRecursoConsumidor
{
    public Task<RecursoConsumidorResponse?> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorConsumidor)
    {
        var recursosConsumidor = Items
            .Where(x => x.Recurso.Descricao == descricaoRecurso && x.Consumidor.Identificador == identificadorConsumidor)
            .Select(x => new RecursoConsumidorResponse(
                Recurso: x.Recurso.Descricao,
                Consumidor: x.Consumidor.Identificador,
                Habilitado: x.Status == EnumStatusRecursoConsumidor.Habilitado
            ))
            .FirstOrDefault();

        return Task.FromResult(recursosConsumidor);
    }


    public IQueryable<RecursoConsumidorResponse> RecuperarPorConsumidor(string identificadorConsumidor)
    {
        var recursoConsumidores = Items
            .Where(x => x.Consumidor.Identificador == identificadorConsumidor)
            .Select(x => new RecursoConsumidorResponse(
                Recurso: x.Recurso.Descricao,
                Consumidor: x.Consumidor.Identificador,
                Habilitado: x.Status == EnumStatusRecursoConsumidor.Habilitado
            ));
        
        return recursoConsumidores.AsQueryable();
    }

    public IQueryable<RecursoConsumidor> RecuperarPorRecurso(string identificadorRecurso)
    {
        return Items
            .Where(x => x.Recurso.Identificador == identificadorRecurso)
            .AsQueryable();
    }
}