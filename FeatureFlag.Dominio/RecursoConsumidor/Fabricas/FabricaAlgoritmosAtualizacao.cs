using FeatureFlag.Domain;
using FeatureFlag.Shared.Extensions;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Dominio.RecursoConsumidor.Fabricas;

public class FabricaAlgoritmosAtualizacao : IFabricaAlgoritmosAtualizacao
{
    #region Ctor
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IRepRecurso _repRecurso;

    public FabricaAlgoritmosAtualizacao(IRepRecursoConsumidor repositorio,
                                        IRepRecurso repRecurso)
    {
        _repRecursoConsumidor = repositorio;
        _repRecurso = repRecurso;
    }
    #endregion

    #region AlgoritmoVitao
    public void AlgoritmoVitao(string identificadorRecurso)
    {
        var porcentagemRecurso = _repRecurso.RecuperarPorcentagemPorIdentificadorAsync(identificadorRecurso).Result;
        
        var consumidores = _repRecursoConsumidor.RecuperarPorRecurso(identificadorRecurso).ToList();
        var totalConsumidores = consumidores.Count;
        var habilitados = _repRecursoConsumidor.RecuperarHabilitadosPorRecurso(identificadorRecurso).Count();

        foreach (var consumidor in consumidores)
        {
            var percentualAtual = (habilitados / (decimal)totalConsumidores) * 100;
            
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

    #region AlgoritmoFisherYates
    public async Task AlgoritmoFisherYates(string identificadorRecurso, int quantidadeAlvo)
    {
        // TODO: SERA ToListAsync
        var habilitados = _repRecursoConsumidor.RecuperarHabilitadosPorRecurso(identificadorRecurso).ToList();
        var desabilitados = _repRecursoConsumidor.RecuperarDesabilitadosPorRecurso(identificadorRecurso).ToList();

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
    }
    
    #region HabilitarConsumidores
    private void HabilitarConsumidores(List<Domain.RecursoConsumidor> desabilitados, int quantidadeRestante)
    {
        var consumidoresParaHabilitar = desabilitados.EmbaralharFisherYates()
            .Take(quantidadeRestante)
            .ToList();

        foreach (var consumidor in consumidoresParaHabilitar)
        {
            consumidor.Habilitar();
        }
    }
    #endregion

    #region DesabilitarConsumidores
    private void DesabilitarConsumidores(List<Domain.RecursoConsumidor> habilitados, int quantidadeParaDesabilitarNegativo)
    {
        var quantidadeParaDesabilitar = NumeroHelper.ValorAbsoluto(quantidadeParaDesabilitarNegativo);

        var consumidoresParaDesabilitar = habilitados.EmbaralharFisherYates()
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
