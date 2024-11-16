using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Domain;

public interface IServRecursoConsumidor : IServBase<RecursoConsumidor, IRepRecursoConsumidor>
{
    int CalcularQuantidadeParaHabilitar(decimal porcentagem, int totalConsumidores);
    Task AtualizarDisponibilidadesAsync(string identificadorRecurso, int quantidadeAlvo);
}