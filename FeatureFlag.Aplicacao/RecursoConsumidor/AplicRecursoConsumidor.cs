    using AutoMapper;
    using FeatureFlag.Aplicacao.Infra;
    using FeatureFlag.Domain;
    using FeatureFlag.Domain.Dtos;
    using FeatureFlag.Dominio;
    using FeatureFlag.Dominio.Dtos;

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
            var porcentagemConfigurada =
                await _servRecurso.Repositorio.RecuperarPorcentagemPorIdentificadorAsync(param.IdentificadorRecurso);
            
            switch (porcentagemConfigurada)
            {
                case 100:
                    return await _servRecursoConsumidor.RetornarCemPorcentoAtivoAsync(param);
                case 0:
                    return await _servRecursoConsumidor.RetornarZeroPorcentoAtivoAsync(param);
            }
            
            var recursoConsumidor = await _servRecursoConsumidor.Repositorio
                .RecuperarPorRecursoConsumidorAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor);

            var disponivel = true;

            throw new NotImplementedException();
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