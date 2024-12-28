﻿using FeatureFlag.Domain;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Dominio;

public class ServConsumidor : ServBase<Consumidor, IRepConsumidor>, IServConsumidor
{
    #region Ctor
    public ServConsumidor(IRepConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion

    #region AdicionarAsync
    public override async Task AdicionarAsync(Consumidor consumidor)
    {
        var consumidorPorIdentificador = await Repositorio.RecuperarPorIdentificadorAsync(consumidor.Identificador);
        if (consumidorPorIdentificador is not null)
        {
            return;
        }
        
        await base.AdicionarAsync(consumidor);
    }
    #endregion
}