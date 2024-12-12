using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Aplicacao;

public class AplicRecurso : AplicBase, IAplicRecurso
{
    #region Ctor
    private readonly IServRecurso _servRecurso;

    public AplicRecurso(IServRecurso servRecurso,
                        IMapper mapper) 
        : base(mapper)
    {
        _servRecurso = servRecurso;
    }
    #endregion
    
    #region AdicionarAsync
    public async Task<RecursoResponse> AdicionarAsync(AdicionarRecursoRequest request)
    {
        var recurso = Mapper.Map<Recurso>(request);

        await IniciarTransacaoAsync();
        await _servRecurso.AdicionarAsync(recurso);
        await PersistirTransacaoAsync();

        var response = Mapper.Map<RecursoResponse>(recurso);

        return response;
    }
    #endregion
    
    #region AlterarAsync
    public async Task<RecursoResponse> AlterarAsync(AlterarRecursoRequest request)
    {
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(request.Identificador);
        recurso.ThrowIfNull("Recurso não foi encontrado.");
        
        var recursoAlterado = Mapper.Map(request, recurso);

        await IniciarTransacaoAsync();
        _servRecurso.Alterar(recursoAlterado);
        await PersistirTransacaoAsync();
        
        var response = Mapper.Map<RecursoResponse>(recursoAlterado);
        
        return response;
    }
    #endregion
}