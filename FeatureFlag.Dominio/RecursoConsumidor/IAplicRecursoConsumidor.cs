﻿using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.RecursoConsumidor.Dtos;

namespace FeatureFlag.Dominio;

public interface IAplicRecursoConsumidor
{
    Task<RecursoConsumidorResponse> RecuperarPorRecursoConsumidorAsync(RecuperarPorRecursoConsumidorParam param);
    Task<List<RecursoConsumidorResponse>> RecuperarPorConsumidorAsync(RecuperarPorConsumidorParam param);
    Task AtualizarHabilitadosAsync();
    Task AtualizarHabilitadosPorRecursoAsync(IdentificadorRecursoRequest request);
}