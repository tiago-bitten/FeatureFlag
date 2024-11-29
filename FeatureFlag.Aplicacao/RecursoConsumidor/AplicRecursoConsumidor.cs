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
            var recursoConsumidor = await _servRecursoConsumidor.Repositorio
                .RecuperarPorRecursoConsumidorAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor);
            
            var porcentagem =
                await _servRecurso.Repositorio.RecuperarPorcentagemPorIdentificadorAsync(param.IdentificadorRecurso);
            
            switch (porcentagem)
            {
                case 100:
                    var estaNaBlacklist = await _servControleAcessoConsumidor.Repositorio
                        .PossuiControleAcessoAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor, EnumTipoControle.Blacklist);
                    
                    return estaNaBlacklist 
                        ? RecursoConsumidorResponse.Desabilitado(param.IdentificadorRecurso, param.IdentificadorConsumidor) 
                        : RecursoConsumidorResponse.Ativo(param.IdentificadorRecurso, param.IdentificadorConsumidor);
                case 0:
                    var estaNaWhitelist = await _servControleAcessoConsumidor.Repositorio
                        .PossuiControleAcessoAsync(param.IdentificadorRecurso, param.IdentificadorConsumidor, EnumTipoControle.Whitelist);
                    
                    return estaNaWhitelist
                        ? RecursoConsumidorResponse.Ativo(param.IdentificadorRecurso, param.IdentificadorConsumidor)
                        : RecursoConsumidorResponse.Desabilitado(param.IdentificadorRecurso, param.IdentificadorConsumidor);
            }

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