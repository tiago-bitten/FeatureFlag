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
            recurso.ExcecaoSeNull("Recurso não encontrado");
            
            await IniciarTransacaoAsync();
            var consumidor = await _servConsumidor.RecuperarPorIdentificadorOuCriarAsync(param.IdentificadorConsumidor);
            await PersistirTransacaoAsync();
            
            switch (recurso.Porcentagem)
            {
                case 100:
                    return await _servRecursoConsumidor.RetornarCemPorcentoAtivoAsync(param);
                case 0:
                    return await _servRecursoConsumidor.RetornarZeroPorcentoAtivoAsync(param);
            }

            await IniciarTransacaoAsync();
            var recursoConsumidor = await _servRecursoConsumidor.RecuperarPorRecursoConsumidorOuCriarAsync(recurso, consumidor);
            await PersistirTransacaoAsync();
            
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
            var recursosConsumidor = _servRecursoConsumidor.Repositorio
                .RecuperarPorConsumidor(param.IdentificadorConsumidor)
                .ToList();
            
            throw new NotImplementedException();
        }
        #endregion
    }