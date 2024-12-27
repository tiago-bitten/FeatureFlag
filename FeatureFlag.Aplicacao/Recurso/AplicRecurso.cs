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
    private readonly IServRecursoConsumidor _servRecursoConsumidor;

    public AplicRecurso(IServRecurso servRecurso,
                        IMapper mapper, 
                        IServRecursoConsumidor servRecursoConsumidor) 
        : base(mapper)
    {
        _servRecurso = servRecurso;
        _servRecursoConsumidor = servRecursoConsumidor;
    }
    #endregion
    
    #region AdicionarAsync
    public async Task<RecursoResponse> AdicionarAsync(AdicionarRecursoRequest request)
    {
        var recurso = Mapper.Map<Recurso>(request);

        await _servRecurso.AdicionarAsync(recurso);

        var response = Mapper.Map<RecursoResponse>(recurso);

        return response;
    }
    #endregion
    
    #region AlterarAsync
    public async Task<RecursoResponse> AlterarAsync(AlterarRecursoRequest request)
    {
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(request.Identificador);
        recurso.ThrowIfNull("Recurso não foi encontrado.");

        var porcentagemAntiga = recurso.Porcentagem;
        
        Mapper.Map(request, recurso);

        await _servRecurso.AtualizarAsync(recurso);
        
        var porcentagemAlteradaEstaMenor = porcentagemAntiga > recurso.Porcentagem;
        if (porcentagemAlteradaEstaMenor)
        {
            await _servRecursoConsumidor.DescongelarTodosPorRecursoAsync(recurso);
        }
        
        var response = Mapper.Map<RecursoResponse>(recurso);
        
        return response;
    }
    #endregion
}