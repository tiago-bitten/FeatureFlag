using AutoMapper;
    using FeatureFlag.Aplicacao.Infra;
    using FeatureFlag.Domain;
    using FeatureFlag.Domain.Dtos;
    using FeatureFlag.Dominio;
    using FeatureFlag.Dominio.Dtos;
    using FeatureFlag.Shared.Extensions;
namespace FeatureFlag.Aplicacao;

public class AplicRecursoConsumidor : AplicBase, IAplicRecursoConsumidor 
{
    #region Ctor
        private readonly IServRecursoConsumidor _servRecursoConsumidor;
        private readonly IServConsumidor _servConsumidor;
        private readonly IServRecurso _servRecurso;
        private readonly IServControleAcessoConsumidor _servControleAcessoConsumidor;

        public AplicRecursoConsumidor(IServRecursoConsumidor servRecursoConsumidor,
                                      IServConsumidor servConsumidor,
                                      IServRecurso servRecurso,
                                      IMapper mapper,
                                      IServControleAcessoConsumidor servControleAcessoConsumidor) 
            : base(mapper)
        {
            _servRecursoConsumidor = servRecursoConsumidor;
            _servConsumidor = servConsumidor;
            _servRecurso = servRecurso;
            _servControleAcessoConsumidor = servControleAcessoConsumidor;
        }
        #endregion
        
    #region RecuperarPorRecursoConsumidorAsync
        public async Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(RecuperarPorRecursoConsumidorParam param)
        {
            var recurso = await _servRecurso.Repositorio.RecuperarPorIdentificadorAsync(param.IdentificadorRecurso);
            recurso.ThrowIfNull("Recurso não foi encontrado.");
            
            var consumidor = await _servConsumidor.Repositorio
                .RecuperarPorIdentificadorAsync(param.IdentificadorConsumidor);
            if (consumidor is null)
            {
                await IniciarTransacaoAsync();
                
                var novoConsumidor = Consumidor.Criar(param.IdentificadorConsumidor);
                await _servConsumidor.AdicionarAsync(novoConsumidor);
                
                await PersistirTransacaoAsync();
            }
            
            switch (recurso.Porcentagem)
            {
                case 100:
                    return await _servRecursoConsumidor.RetornarCemPorcentoAtivoAsync(param);
                case 0:
                    return await _servRecursoConsumidor.RetornarZeroPorcentoAtivoAsync(param);
            }

            var recursoConsumidor = await _servRecursoConsumidor.Repositorio
                .RecuperarPorRecursoConsumidorAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor, "Recurso", "Consumidor");
            if (recursoConsumidor is null)
            {
                await IniciarTransacaoAsync();
                
                // Remover padrão factory e utilizar constutores com inicialização
                var novoRecursoConsumidor = RecursoConsumidor.Criar(recurso.Id, consumidor.Id);
                novoRecursoConsumidor.Recurso = recurso;
                novoRecursoConsumidor.Consumidor = consumidor;
                await _servRecursoConsumidor.AdicionarAsync(novoRecursoConsumidor);
                
                await PersistirTransacaoAsync();
            }
            
            await IniciarTransacaoAsync();
            await _servRecursoConsumidor.AtualizarStatusAsync(recursoConsumidor);
            await PersistirTransacaoAsync();
            
            var response = Mapper.Map<RecursoConsumidorResponse>(recursoConsumidor);

            return response;
        }
        #endregion
        
    #region RecuperarPorConsumidorAsync
        public Task<List<RecursoConsumidorResponse>> RecuperarPorConsumidorAsync(RecuperarPorConsumidorParam param)
        {
            throw new NotImplementedException();
        }
        #endregion
}