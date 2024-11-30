using AutoMapper;
using AutoMapper.QueryableExtensions;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Repositorio.Infra;

namespace FeatureFlag.Repositorio;

public class RepMemoRecursoConsumidor : RepMemoBase<RecursoConsumidor>, IRepRecursoConsumidor
{
    private readonly IRepControleAcessoConsumidor _repControleAcessoConsumidor;
    
    public RepMemoRecursoConsumidor(IRepControleAcessoConsumidor repControleAcessoConsumidor,
                                    IMapper mapper)
        : base(mapper)
    {
        _repControleAcessoConsumidor = repControleAcessoConsumidor;
    }
    
    public Task<RecursoConsumidor?> RecuperarPorRecursoConsumidorAsync(string identificadorRecurso, string identificadorConsumidor)
    {
        var query = Items.AsQueryable();
        
        var recursosConsumidor = query
            .FirstOrDefault(x => x.Recurso.Identificador == identificadorRecurso && x.Consumidor.Identificador == identificadorConsumidor);

        return Task.FromResult(recursosConsumidor);
    }


    public IQueryable<RecursoConsumidor> RecuperarPorConsumidor(string identificadorConsumidor)
    {
        var query = Items.AsQueryable();

        var recursoConsumidores = query
            .Where(x => x.Consumidor.Identificador == identificadorConsumidor);
        
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