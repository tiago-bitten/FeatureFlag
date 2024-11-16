using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public interface IServRecursoConsumidor : IServBase<RecursoConsumidor, IRepRecursoConsumidor>
{
    Task<int> CalcularQuantidadeParaHabilitarAsync(string identificadorRecurso);
    Task AtualizarDisponibilidadesAsync(string identificadorRecurso, int quantidadeAlvo);
}