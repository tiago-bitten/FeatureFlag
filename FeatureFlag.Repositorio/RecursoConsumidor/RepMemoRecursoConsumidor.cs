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
    
    public Task<RecursoConsumidor?> RecuperarPorRecursoConsumidorAsync(string identificadorRecurso, string identificadorConsumidor, params string[]? includes)
    {
        var query = Items.AsQueryable();
        
        var recursosConsumidor = query
            .FirstOrDefault(x => x.Recurso.Identificador == identificadorRecurso && x.Consumidor.Identificador == identificadorConsumidor);

        return Task.FromResult(recursosConsumidor);
    }


    public IQueryable<RecursoConsumidor> RecuperarPorConsumidor(string identificadorConsumidor, params string[]? includes)
    {
        var query = Items.AsQueryable();

        var recursoConsumidores = query
            .Where(x => x.Consumidor.Identificador == identificadorConsumidor);
        
        return recursoConsumidores.AsQueryable();
    }

    public IQueryable<RecursoConsumidor> RecuperarPorRecurso(string identificadorRecurso, params string[]? includes)
    {
        return Items
            .Where(x => x.Recurso.Identificador == identificadorRecurso)
            .AsQueryable();
    }

    public IQueryable<RecursoConsumidor> RecuperarPorStatus(EnumStatusRecursoConsumidor status, params string[]? includes)
    {
        return Items
            .Where(x => x.Status == status)
            .AsQueryable();
    }

    public IQueryable<RecursoConsumidor> RecuperarPorTipo(string identificadorRecurso, EnumTipoControle tipo, params string[]? includes)
    {
        throw new NotImplementedException();
    }
}