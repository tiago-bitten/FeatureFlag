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

        public AplicRecursoConsumidor(IServRecursoConsumidor servRecursoConsumidor,
                                      IServConsumidor servConsumidor,
                                      IServRecurso servRecurso,
                                      IMapper mapper) 
            : base(mapper)
        {
            _servRecursoConsumidor = servRecursoConsumidor;
            _servConsumidor = servConsumidor;
            _servRecurso = servRecurso;
        }
        #endregion

        #region RecuperarPorRecursoConsumidorAsync
        public async Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(RecuperarPorRecursoConsumidorParam param)
        {
            var porcentagem =
                await _servRecurso.Repositorio.RecuperarPorcentagemPorIdentificadorAsync(param.IdentificadorRecurso);

            switch (porcentagem)
            {
                case 100:
                    // 1. Verificar se o consumidor está na blacklist;
                    // 2. Se estiver, retornar que o recurso está desabilitado;
                    // 3. Se não estiver, retornar que o recurso está habilitado;
                    break;
                case 0:
                    // 1. Verificar se o consumidor está na whitelist;
                    // 2. Se estiver, retornar que o recurso está habilitado;
                    // 3. Se não estiver, retornar que o recurso está desabilitado;
                    break;
            }
            
            var recursoConsumidor = await _servRecursoConsumidor.Repositorio
                .RecuperarPorRecursoConsumidorAsync(param.IdentificadorRecurso, param.IdentificadorRecurso);

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