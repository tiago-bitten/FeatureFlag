using FeatureFlag.Dominio.Infra;
using MongoDB.Bson;

namespace FeatureFlag.Dominio;

public interface IRepControleAcessoConsumidor : IRepBase<ControleAcessoConsumidor>
{
    Task<bool> PossuiPorTipoAsync(string identificadorRecurso, string identificadorConsumidor, EnumTipoControle tipoControle);
    Task<List<ControleAcessoConsumidor>> RecuperarPorRecursoAsync(ObjectId codigoRecurso);
}