using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public class ServConsumidor : ServBase<Consumidor, IRepConsumidor>, IServConsumidor
{
    #region Ctor
    public ServConsumidor(IRepConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion
    
    #region RecuperarPorIdentificadorAsync
    public async Task<Consumidor> RecuperarPorIdentificadorAsync(string identificador)
    {
        var consumidor = await Repositorio.RecuperarPorIdentificadorAsync(identificador);

        if (consumidor is null)
        {
            consumidor = Consumidor.Criar(identificador);
            await AdicionarAsync(consumidor);
        }
        return consumidor;
    }
    #endregion
}