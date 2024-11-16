using FeatureFlag.Domain.Infra;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    public ServRecursoConsumidor(IRepRecursoConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion

    #region CalcularDisponibilidadesAsync
    public int CalcularQuantidadeParaHabilitar(decimal recursoPorcentagem, int totalConsumidores)
    {
        return (int)NumeroHelper.Arredondar(totalConsumidores * recursoPorcentagem / 100);
    }
    #endregion

    #region 
    public Task AtualizarDisponibilidadesAsync(string identificadorRecurso, int quantidadeAlvo)
    {
        throw new NotImplementedException();
    }
    #endregion
}