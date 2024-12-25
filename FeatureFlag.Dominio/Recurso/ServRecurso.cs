using FeatureFlag.Domain.Dtos;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Extensions;

namespace FeatureFlag.Domain;

public class ServRecurso : ServBase<Recurso, IRepRecurso>, IServRecurso
{
    #region Ctor
    public ServRecurso(IRepRecurso repositorio) : base(repositorio)
    {
    }
    #endregion
    
    #region AlterarPorcentagemAsync
    public async Task<Recurso> AlterarPorcentagemAsync(Recurso recurso, decimal novaPorcentagem)
    {
        recurso.Porcentagem.Atualizar(novaPorcentagem);
        await AtualizarAsync(recurso);
        
        return recurso;
    }
    #endregion
}