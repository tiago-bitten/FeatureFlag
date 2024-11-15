using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;

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
    public async Task<RecursoResponse> AdicionarAsync(CriarRecursoRequest request)
    {
        var recurso = Mapear<CriarRecursoRequest, Recurso>(request);

        await IniciarTransacaoAsync();
        await _servRecurso.AdicionarAsync(recurso);
        await PersistirTransacaoAsync();

        var response = Mapear<Recurso, RecursoResponse>(recurso);

        return response;
    }
    #endregion
}