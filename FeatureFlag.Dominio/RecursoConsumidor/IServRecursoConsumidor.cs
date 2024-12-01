﻿using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Domain;

public interface IServRecursoConsumidor : IServBase<RecursoConsumidor, IRepRecursoConsumidor>
{
    Task<RecursoConsumidor> RecuperarPorRecursoConsumidorOuCriarAsync(Recurso recurso, Consumidor consumidor);
    Task AtualizarStatusAsync(RecursoConsumidor recursoConsumidor);
    Task<RecursoConsumidorResponse> RetornarCemPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param);
    Task<RecursoConsumidorResponse> RetornarZeroPorcentoAtivoAsync(RecuperarPorRecursoConsumidorParam param);
}