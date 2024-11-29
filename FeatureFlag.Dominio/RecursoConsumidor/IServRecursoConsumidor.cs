using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecursoConsumidor : IServBase<RecursoConsumidor, IRepRecursoConsumidor>
{
    Task AtualizarDisponibilidadeAsync();
}