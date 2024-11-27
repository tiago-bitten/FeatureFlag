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
    
    public Task<RecursoConsumidorResponse?> RecuperarPorRecursoConsumidorAsync(string descricaoRecurso, string identificadorConsumidor)
    {
        var query = Items.AsQueryable();
        
        var recursosConsumidor = query
            .Where(x => x.Recurso.Descricao == descricaoRecurso && x.Consumidor.Identificador == identificadorConsumidor)
            .ProjectTo<RecursoConsumidorResponse>(Mapper.ConfigurationProvider)
            .FirstOrDefault();

        return Task.FromResult(recursosConsumidor);
    }


    public IQueryable<RecursoConsumidorResponse> RecuperarPorConsumidor(string identificadorConsumidor)
    {
        var query = Items.AsQueryable();

        var recursoConsumidores = query
            .Where(x => x.Consumidor.Identificador == identificadorConsumidor)
            .ProjectTo<RecursoConsumidorResponse>(Mapper.ConfigurationProvider);
        
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