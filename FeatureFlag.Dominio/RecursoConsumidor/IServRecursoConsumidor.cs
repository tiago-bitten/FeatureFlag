using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecursoConsumidor : IServBase<RecursoConsumidor, IRepRecursoConsumidor>
{
    Task AtualizarDisponibilidadeAsync(RecursoConsumidor recursoConsumidor, Recurso recurso, Consumidor consumidor);
    Task DescongelarTodosPorRecursoAsync(Recurso recurso);
    RecursoConsumidorResponse RetornarComControleAcesso(Consumidor consumidor, Recurso recurso);
    RecursoConsumidorResponse RetornarCemPorcentoAtivo(Consumidor consumidor, Recurso recurso);
    RecursoConsumidorResponse RetornarZeroPorcentoAtivo(Consumidor consumidor, Recurso recurso);
    Task RemoverPorRecursoAsync(Recurso recurso);
}