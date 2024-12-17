using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public interface IRepControleAcessoConsumidor : IRepBase<ControleAcessoConsumidor>
{
    Task<bool> PossuiPorTipoAsync(string identificadorRecurso, string identificadorConsumidor, EnumTipoControle tipoControle);
}