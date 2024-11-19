using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Repositorio.Infra;

namespace FeatureFlag.Repositorio;

public class RepMemoRecursoConsumidor : RepMemoBase<RecursoConsumidor>, IRepRecursoConsumidor
{
    private readonly IRepControleAcessoConsumidor _repControleAcessoConsumidor;
    
    public RepMemoRecursoConsumidor(IRepControleAcessoConsumidor repControleAcessoConsumidor)
    {
        _repControleAcessoConsumidor = repControleAcessoConsumidor;
    }
    
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

    public IQueryable<RecursoConsumidor> RecuperarHabilitadosPorRecurso(string identificadorRecurso)
    {
        return RecuperarPorRecurso(identificadorRecurso)
            .Where(x => x.Status == EnumStatusRecursoConsumidor.Habilitado);
    }
    
    public IQueryable<RecursoConsumidor> RecuperarDesabilitadosPorRecurso(string identificadorRecurso)
    {
        return RecuperarPorRecurso(identificadorRecurso)
            .Where(x => x.Status == EnumStatusRecursoConsumidor.Desabilitado);
    }

    public IQueryable<RecursoConsumidor> RecuperarPorTipo(string identificadorRecurso, EnumTipoControle tipo)
    {
        throw new NotImplementedException();
    }
}