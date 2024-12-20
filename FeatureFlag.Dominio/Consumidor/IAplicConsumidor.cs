﻿using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Dominio;

public interface IAplicConsumidor
{
    Task<ConsumidorResponse> AdicionarAsync(CriarConsumidorRequest request);
    Task<ConsumidorResponse> AlterarAsync(AlterarConsumidorRequest request);
}