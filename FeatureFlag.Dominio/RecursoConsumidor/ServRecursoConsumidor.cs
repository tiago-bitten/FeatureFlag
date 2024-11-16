using FeatureFlag.Domain.Infra;
using FeatureFlag.Dominio;
using FeatureFlag.Shared.Extensions;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    private readonly IRepRecurso _repRecurso;
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepControleAcessoConsumidor _repControleAcessoConsumidor;

    public ServRecursoConsumidor(IRepRecursoConsumidor repositorio,
                                 IRepRecurso repRecurso,
                                 IRepConsumidor repConsumidor,
                                 IRepControleAcessoConsumidor repControleAcessoConsumidor)
        : base(repositorio)
    {
        _repRecurso = repRecurso;
        _repConsumidor = repConsumidor;
        _repControleAcessoConsumidor = repControleAcessoConsumidor;
    }
    #endregion

    #region CalcularDisponibilidadesAsync
    public async Task<int> CalcularQuantidadeParaHabilitarAsync(string identificadorRecurso)
    {
        var porcentagemRecurso = await _repRecurso.RecuperarPorcentagemPorIdentificadorAsync(identificadorRecurso);
        var totalConsumidores = await _repConsumidor.CountAsync();

        if (porcentagemRecurso is 0 || totalConsumidores is 0)
            return 0;

        return (int)NumeroHelper.Arredondar(totalConsumidores * porcentagemRecurso / 100);
    }
    #endregion

    #region AtualizarDisponibilidadesAsync
    public Task AtualizarDisponibilidadesAsync(string identificadorRecurso, int quantidadeAlvo)
    {
        var habilitados = Repositorio.RecuperarHabilitadosPorRecurso(identificadorRecurso).ToList();
        var desabilitados = Repositorio.RecuperarDesabilitadosPorRecurso(identificadorRecurso).ToList();
        var quantidadeRestante = quantidadeAlvo - habilitados.Count;

        switch (quantidadeRestante)
        {
            case > 0:
                HabilitarConsumidores(desabilitados, quantidadeRestante);
                break;

            case < 0:
                DesabilitarConsumidores(habilitados, quantidadeRestante);
                break;
        }

        var whitelist = _repControleAcessoConsumidor.RecuperarPorTipo(identificadorRecurso, EnumTipoControle.Whitelist)
            .SelectMany(x => x.Consumidor.RecursoConsumidores.Where(rc => rc.Consumidor.Identificador == identificadorRecurso))
            .ToList();

        var blacklist = _repControleAcessoConsumidor.RecuperarPorTipo(identificadorRecurso, EnumTipoControle.Blacklist)
            .SelectMany(x => x.Consumidor.RecursoConsumidores.Where(rc => rc.Recurso.Identificador == identificadorRecurso))
            .ToList();

        HabilitarTodosConsumidores(whitelist);
        DesabilitarTodosConsumidores(blacklist);

        return Task.CompletedTask;
    }

    #region HabilitarConsumidores
    private void HabilitarConsumidores(List<RecursoConsumidor> desabilitados, int quantidadeRestante)
    {
        var consumidoresParaHabilitar = desabilitados.EmbaralharFisherYates()
            .Take(quantidadeRestante)
            .ToList();
        
        HabilitarTodosConsumidores(consumidoresParaHabilitar);
    }
    #endregion

    #region DesabilitarConsumidores
    private void DesabilitarConsumidores(List<RecursoConsumidor> habilitados, int quantidadeParaDesabilitarNegativo)
    {
        var quantidadeParaDesabilitar = NumeroHelper.ValorAbsoluto(quantidadeParaDesabilitarNegativo);
        var consumidoresParaDesabilitar = habilitados.EmbaralharFisherYates()
            .Take(quantidadeParaDesabilitar)
            .ToList();

        DesabilitarTodosConsumidores(consumidoresParaDesabilitar);
    }
    #endregion

    #region HabilitarTodosConsumidores
    private void HabilitarTodosConsumidores(List<RecursoConsumidor> consumidores)
    {
        foreach (var consumidor in consumidores)
        {
            consumidor.Habilitar();
        }
    }
    #endregion

    #region DesabilitarTodosConsumidores
    private void DesabilitarTodosConsumidores(List<RecursoConsumidor> consumidores)
    {
        foreach (var consumidor in consumidores)
        {
            consumidor.Desabilitar();
        }
    }
    #endregion
    #endregion

    #region AlgoritmoVitao
    private async Task AlgoritmoVitao(string identificadorRecurso)
    {
        var porcentagemRecurso = await _repRecurso.RecuperarPorcentagemPorIdentificadorAsync(identificadorRecurso);
        var consumidores = Repositorio.RecuperarPorRecurso(identificadorRecurso).ToList();
        var totalConsumidores = consumidores.Count;
        var habilitados = Repositorio.RecuperarHabilitadosPorRecurso(identificadorRecurso).Count();

        foreach (var consumidor in consumidores)
        {
            var percentualAtual = habilitados / (decimal)totalConsumidores * 100;

            if (percentualAtual < porcentagemRecurso)
            {
                if (consumidor.Status == EnumStatusRecursoConsumidor.Habilitado) continue;
                consumidor.Habilitar();
                habilitados++;
            }
            else
            {
                if (consumidor.Status == EnumStatusRecursoConsumidor.Desabilitado) continue;
                consumidor.Desabilitar();
                habilitados--;
            }
        }
    }
    #endregion
}
