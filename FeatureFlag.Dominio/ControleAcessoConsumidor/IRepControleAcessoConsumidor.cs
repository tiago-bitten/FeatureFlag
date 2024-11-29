using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IRepControleAcessoConsumidor : IRepBase<ControleAcessoConsumidor>
{
    IQueryable<ControleAcessoConsumidor> RecuperarPorTipo(string identificadorRecurso, EnumTipoControle tipo);
    Task<bool> PossuiControleAcessoAsync(string identificadorRecurso, string identificadorConsumidor, EnumTipoControle tipo);
}