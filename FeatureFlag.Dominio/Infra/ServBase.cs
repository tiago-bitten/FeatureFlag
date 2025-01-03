﻿namespace FeatureFlag.Dominio.Infra;

public abstract class ServBase<TEntidade, TRepositorio> : IServBase<TEntidade, TRepositorio>
    where TEntidade : EntidadeBase
    where TRepositorio : IRepBase<TEntidade>
{
    #region Ctor
    public TRepositorio Repositorio { get; }

    public ServBase(TRepositorio repositorio)
    {
        Repositorio = repositorio;
    }
    #endregion

    #region AdicionarAsync
    public virtual async Task AdicionarAsync(TEntidade entidade)
    {
        await Repositorio.AdicionarAsync(entidade);
    }
    #endregion
    
    #region AlterarAsync
    public virtual async Task AtualizarAsync(TEntidade entidade)
    {
        entidade.Alterar();
        await Repositorio.AtualizarAsync(entidade);
    }
    #endregion
    
    #region AtualizarVariosAsync
    public virtual async Task AtualizarVariosAsync(List<TEntidade> entidades)
    {
        if (entidades.Count == 0)
        {
            return;
        }        
        await Repositorio.AtualizarVariosAsync(entidades);
    }
    #endregion
    
    #region RemoverAsync
    public virtual async Task RemoverAsync(TEntidade entidade)
    {
        entidade.Inativar();
        await Repositorio.AtualizarAsync(entidade);
    }
    #endregion
}