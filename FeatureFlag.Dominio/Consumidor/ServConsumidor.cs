using FeatureFlag.Dominio.Infra;

namespace FeatureFlag.Dominio;

public class ServConsumidor : ServBase<Consumidor, IRepConsumidor>, IServConsumidor
{
    #region Ctor
    public ServConsumidor(IRepConsumidor repositorio) : base(repositorio)
    {
    }
    #endregion

    public override async Task AdicionarAsync(Consumidor consumidor)
    {
        var consumidorPorIdentificador = await Repositorio.RecuperarPorIdentificadorAsync(consumidor.Identificador);
        if (consumidorPorIdentificador is not null)
        {
            consumidor.AlterarDados(consumidorPorIdentificador.Identificador, consumidorPorIdentificador.Descricao);
            return;
        }
        
        await base.AdicionarAsync(consumidor);
    }
}