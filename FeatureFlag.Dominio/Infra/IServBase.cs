﻿namespace FeatureFlag.Dominio.Infra;

public interface IServBase<TEntidade, TRepositorio>
    where TEntidade : EntidadeBase
    where TRepositorio : IRepBase<TEntidade>
{
    TRepositorio Repositorio { get; }
    
    Task AdicionarAsync(TEntidade entidade);
    Task AlterarAsync(TEntidade entidade);
}