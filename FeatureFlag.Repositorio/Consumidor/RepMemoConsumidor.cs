using FeatureFlag.Dominio;
using FeatureFlag.Repositorio.Infra;

namespace FeatureFlag.Repositorio;

public class RepMemoConsumidor : RepMemoBase<Consumidor>, IRepConsumidor
{
    public Task<Consumidor?> RecuperarPorIdentificadorAsync(string identificador)
    {
        return Task.FromResult(Items
            .FirstOrDefault(x => x.Identificador == identificador));
    }
}