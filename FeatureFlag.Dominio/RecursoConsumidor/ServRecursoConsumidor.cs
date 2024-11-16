using FeatureFlag.Domain.Infra;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    private readonly IRepRecurso _repRecurso;
    private readonly IRepConsumidor _repConsumidor;
    
    public ServRecursoConsumidor(IRepRecursoConsumidor repositorio,
                                 IRepRecurso repRecurso,
                                 IRepConsumidor repConsumidor) 
        : base(repositorio)
    {
        _repRecurso = repRecurso;
        _repConsumidor = repConsumidor;
    }
    #endregion

    #region CalcularDisponibilidadesAsync
    public async Task<int> CalcularQuantidadeParaHabilitarAsync(string identificadorRecurso)
    {
        var porcentagemRecurso = await _repRecurso.RecuperarPorcentagemPorIdentificadorAsync(identificadorRecurso);
        var totalConsumidores = await _repConsumidor.CountAsync();
        
        return (int)NumeroHelper.Arredondar(totalConsumidores * porcentagemRecurso / 100);
    }
    #endregion

    #region AtualizarDisponibilidadesAsync
    public async Task AtualizarDisponibilidadesAsync(string identificadorRecurso, int quantidadeAlvo)
    {
        var total = Repositorio.RecuperarPorRecurso(identificadorRecurso).ToList();
        var habilitados = Repositorio.RecuperarHabilitadosPorRecurso(identificadorRecurso).ToList();

        var quantidadeRestante = quantidadeAlvo - habilitados.Count;

        switch (quantidadeRestante)
        {
            case > 0:
                HabilitarConsumidores(total, quantidadeRestante);
                break;
            
            case < 0:
                DesabilitarConsumidores(habilitados, Math.Abs(quantidadeRestante));
                break;
        }
    }

    
    #region HabilitarConsumidores
    private void HabilitarConsumidores(List<RecursoConsumidor> desabilitados, int quantidadeRestante)
    {
        var random = new Random();
        var consumidoresParaHabilitar = desabilitados
            .Where(rc => rc.Status == EnumStatusRecursoConsumidor.Desabilitado)
            .OrderBy(_ => random.Next())
            .Take(quantidadeRestante)
            .ToList();

        foreach (var consumidor in consumidoresParaHabilitar)
        {
            consumidor.Habilitar();
        }
    }
    #endregion

    #region DesabilitarConsumidores
    private void DesabilitarConsumidores(List<RecursoConsumidor> habilitados, int quantidadeParaDesabilitar)
    {
        var random = new Random();
        var consumidoresParaDesabilitar = habilitados
            .OrderBy(_ => random.Next())
            .Take(quantidadeParaDesabilitar)
            .ToList();

        foreach (var consumidor in consumidoresParaDesabilitar)
        {
            consumidor.Desabilitar();
        }
    }
    #endregion
    #endregion
}
