using AutoMapper;
    using FeatureFlag.Aplicacao.Infra;
    using FeatureFlag.Domain;
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
                consumidor = Consumidor.Criar(param.IdentificadorConsumidor);
                await _servConsumidor.AdicionarAsync(consumidor);
            }
            
            switch (recurso.Porcentagem)
            {
                case 100:
                    return await _servRecursoConsumidor.RetornarCemPorcentoAtivoAsync(param);
                case 0:
                    return await _servRecursoConsumidor.RetornarZeroPorcentoAtivoAsync(param);
            }

            var recursoConsumidor = await _servRecursoConsumidor.Repositorio
                .RecuperarPorRecursoEConsumidorAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor);
            if (recursoConsumidor is null)
            {
                // Remover padrão factory e utilizar constutores com inicialização
                recursoConsumidor = new RecursoConsumidor(recurso, consumidor);
                await _servRecursoConsumidor.AdicionarAsync(recursoConsumidor);
            }
            
            await _servRecursoConsumidor.AtualizarStatusAsync(recursoConsumidor);
            
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