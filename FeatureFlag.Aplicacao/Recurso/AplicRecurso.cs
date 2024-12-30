using AutoMapper;
using FeatureFlag.Aplicacao.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Aplicacao;

public class AplicRecurso : AplicBase, IAplicRecurso
{
    #region Ctor
    private readonly IServRecurso _servRecurso;
    private readonly IServRecursoConsumidor _servRecursoConsumidor;
    private readonly IServControleAcessoConsumidor _servControleAcessoConsumidor;
    private readonly IServConsumidor _servConsumidor;

    public AplicRecurso(IServRecurso servRecurso,
                        IMapper mapper, 
                        IServRecursoConsumidor servRecursoConsumidor, 
                        IServControleAcessoConsumidor servControleAcessoConsumidor,
                        IServConsumidor servConsumidor) 
        : base(mapper)
    {
        _servRecurso = servRecurso;
        _servRecursoConsumidor = servRecursoConsumidor;
        _servControleAcessoConsumidor = servControleAcessoConsumidor;
        _servConsumidor = servConsumidor;
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
    public async Task<RecursoResponse> AlterarAsync(string identificador, AlterarRecursoRequest request)
    {
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(identificador);
        recurso.ThrowIfNull("Recurso não foi encontrado.");

        var porcentagemAntiga = recurso.Porcentagem;
        
        Mapper.Map(request, recurso);

        await _servRecurso.AtualizarAsync(recurso);
        await SinconizarEmbeddedsAsync(new SincronizarEmbeddeds<Recurso>(recurso));
        
        var porcentagemAlteradaEstaMenor = recurso.Porcentagem < porcentagemAntiga;
        if (porcentagemAlteradaEstaMenor)
        {
            await _servRecursoConsumidor.DescongelarTodosPorRecursoAsync(recurso);
        }
        
        var response = Mapper.Map<RecursoResponse>(recurso);
        
        return response;
    }
    #endregion
    
    #region RecuperarTodosAsync
    public async Task<List<RecursoResponse>> RecuperarTodosAsync()
    {
        var recursos = await _servRecurso.Repositorio.RecuperarTodosAsync();
        
        var response = Mapper.Map<List<RecursoResponse>>(recursos);
        
        return response;
    }
    #endregion
    
    #region RecuperarPorIdentificadorAsync
    public async Task<RecursoResponse> RecuperarPorIdentificadorAsync(string identificador)
    {
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(identificador);
        recurso.ThrowIfNull("Recurso não foi encontrado.");

        var response = Mapper.Map<RecursoResponse>(recurso);

        return response;
    }
    #endregion
    
    #region RemoverAsync
    public async Task RemoverAsync(string identificador)
    {
        var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(identificador);
        recurso.ThrowIfNull("Recurso não foi encontrado.");

        var consumidores = await _servConsumidor.Repositorio.RecuperarTodosAsync();
        foreach (var consumidor in consumidores)
        {
            consumidor.RemoverControleAcesso(identificador);
        }
        await _servConsumidor.AtualizarVariosAsync(consumidores);

        await _servRecursoConsumidor.RemoverPorRecursoAsync(recurso);
        await _servControleAcessoConsumidor.RemoverPorRecursoAsync(recurso);
        await _servRecurso.RemoverAsync(recurso);
    }
    #endregion
    
    #region SinconizarEmbeddedAsync
    public async Task SinconizarEmbeddedsAsync(SincronizarEmbeddeds<Recurso> recurso)
    {
        var recursoAtualizado = recurso.Entidade;

        var consumidores = await _servConsumidor.Repositorio.RecuperarTodosAsync();
        _servRecurso.SincronizarConsumidores(recursoAtualizado, consumidores);
        await _servConsumidor.AtualizarVariosAsync(consumidores);
        
        var recursoConsumidores = await _servRecursoConsumidor.Repositorio.RecuperarPorRecursoAsync(recursoAtualizado.Id);
        _servRecurso.SincronizarRecursoConsumidores(recursoAtualizado, recursoConsumidores);
        await _servRecursoConsumidor.AtualizarVariosAsync(recursoConsumidores);
        
        var controleAcessoConsumidores = await _servControleAcessoConsumidor.Repositorio.RecuperarPorRecursoAsync(recursoAtualizado.Id);
        _servRecurso.SincronizarControleAcessoConsumidores(recursoAtualizado, controleAcessoConsumidores);
        await _servControleAcessoConsumidor.AtualizarVariosAsync(controleAcessoConsumidores);
    }

    #endregion
}