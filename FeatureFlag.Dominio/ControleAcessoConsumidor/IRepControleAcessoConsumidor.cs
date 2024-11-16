﻿using FeatureFlag.Domain.Infra;

namespace FeatureFlag.Dominio;

public interface IRepControleAcessoConsumidor : IRepBase<ControleAcessoConsumidor>
{
    IQueryable<ControleAcessoConsumidor> RecuperarPorTipo(string identificadorRecurso, EnumTipoControle tipo);
}