﻿using FeatureFlag.Domain.Infra;
using FeatureFlag.Shared.Extensions;
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

        if (porcentagemRecurso is 0 || totalConsumidores is 0)
            return 0;
        
        return (int)NumeroHelper.Arredondar(totalConsumidores * porcentagemRecurso / 100);
    }
    #endregion

    #region AtualizarDisponibilidadesAsync
    public Task AtualizarDisponibilidadesAsync(string identificadorRecurso, int quantidadeAlvo)
    {
        // TODO: SERA ToListAsync
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
        
        return Task.CompletedTask;
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
    private void DesabilitarConsumidores(List<RecursoConsumidor> habilitados, int quantidadeParaDesabilitarNegativo)
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
    
    #region AlgoritmoVitao
    private async Task AlgoritmoVitao(string identificadorRecurso)
    {
        var porcentagemRecurso = await _repRecurso.RecuperarPorcentagemPorIdentificadorAsync(identificadorRecurso);
        
        var consumidores = Repositorio.RecuperarPorRecurso(identificadorRecurso).ToList();
        var totalConsumidores = consumidores.Count;
        var habilitados = Repositorio.RecuperarHabilitadosPorRecurso(identificadorRecurso).Count();

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

    #endregion
}
