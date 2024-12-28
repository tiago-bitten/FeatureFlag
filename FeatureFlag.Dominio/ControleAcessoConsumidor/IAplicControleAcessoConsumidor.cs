using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Dominio;

public interface IAplicControleAcessoConsumidor
{
    Task<ControleAcessoConsumidorResponse> AdicionarAsync(AdicionarControleAcessoConsumidorRequest request);
    Task<List<ControleAcessoConsumidorResponse>> RecuperarPorConsumidorAsync(string identificadorConsumidor);
    Task RemoverPorRecursoConsumidorAsync(RemoverPorRecursoConsumidorParam param);
}