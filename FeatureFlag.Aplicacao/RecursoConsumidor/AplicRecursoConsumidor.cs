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
            var recursoConsumidor = await _servRecursoConsumidor.Repositorio
                .RecuperarPorRecursoConsumidorAsync(param.IdentificadorRecurso, param.IdentificadorRecurso);

            if (recursoConsumidor is not null)
                return recursoConsumidor;
            
            var novoConsumidor = Consumidor.Criar(param.IdentificadorConsumidor);

            await IniciarTransacaoAsync();
            await _servConsumidor.AdicionarAsync(novoConsumidor);
            await PersistirTransacaoAsync();
            
            return RecursoConsumidorResponse.ConsumidorSemRecurso(novoConsumidor, param.IdentificadorRecurso);
        }
        #endregion
        
        #region RecuperarPorConsumidorAsync
        public Task<List<RecursoConsumidorResponse>> RecuperarPorConsumidorAsync(RecuperarPorConsumidorParam param)
        {
            var recursosConsumidor = _servRecursoConsumidor.Repositorio
                .RecuperarPorConsumidor(param.IdentificadorConsumidor)
                .ToList();

            return Task.FromResult(recursosConsumidor);
        }
        #endregion

        public async Task AtualizarHabilitadosAsync()
        {
            var identificadoresRecursos = _servRecurso.Repositorio
                .RecuperarTodos()
                .Select(x => x.Identificador)
                .ToList();

            await IniciarTransacaoAsync();
            foreach (var identificador in identificadoresRecursos)
            {
                var quantidadeParaHabilitar = await _servRecursoConsumidor.CalcularQuantidadeParaHabilitarAsync(identificador);
                await _servRecursoConsumidor.AtualizarDisponibilidadesAsync(identificador, quantidadeParaHabilitar);
            }
            await PersistirTransacaoAsync();
        }

        public async Task AtualizarHabilitadosPorRecursoAsync(IdentificadorRecursoRequest request)
        {
            var quantidadeParaHabilitar = await _servRecursoConsumidor.CalcularQuantidadeParaHabilitarAsync(request.IdentificadorRecurso);

            await IniciarTransacaoAsync();
            await _servRecursoConsumidor.AtualizarDisponibilidadesAsync(request.IdentificadorRecurso, quantidadeParaHabilitar);
            await PersistirTransacaoAsync();
        }
    }