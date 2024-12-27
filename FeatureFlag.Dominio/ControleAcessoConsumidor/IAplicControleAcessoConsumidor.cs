using FeatureFlag.Dominio.Dtos;

namespace FeatureFlag.Dominio;

public interface IAplicControleAcessoConsumidor
{
    Task<ControleAcessoConsumidorResponse> AdicionarAsync(AdicionarControleAcessoConsumidorRequest request);
    Task<List<ControleAcessoConsumidorResponse>> RecuperarPorConsumidorAsync(int codigoConsumidor);
}