using FeatureFlag.Domain.Infra;
using FeatureFlag.Dominio.RecursoConsumidor.Fabricas;
using FeatureFlag.Shared.Extensions;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public class ServRecursoConsumidor : ServBase<RecursoConsumidor, IRepRecursoConsumidor>, IServRecursoConsumidor
{
    #region Ctor
    private readonly IRepRecurso _repRecurso;
    private readonly IRepConsumidor _repConsumidor;
    private readonly IFabricaAlgoritmosAtualizacao _fabricaAlgoritmosAtualizacao;
    
    public ServRecursoConsumidor(IRepRecursoConsumidor repositorio,
                                 IRepRecurso repRecurso,
                                 IRepConsumidor repConsumidor, 
                                 IFabricaAlgoritmosAtualizacao fabricaAlgoritmosAtualizacao) 
        : base(repositorio)
    {
        _repRecurso = repRecurso;
        _repConsumidor = repConsumidor;
        _fabricaAlgoritmosAtualizacao = fabricaAlgoritmosAtualizacao;
    }
    #endregion

    #region CalcularDisponibilidadesAsync
    public async Task<int> CalcularQuantidadeParaHabilitarAsync(string identificadorRecurso)
    {
        var porcentagemRecurso = await _repRecurso.RecuperarPorcentagemPorIdentificadorAsync(identificadorRecurso);
        var totalConsumidores = await _repConsumidor.CountAsync();

        if (porcentagemRecurso is 0 || totalConsumidores is 0)
            return 0;
        
        return (int)NumeroHelper.Arredondar(totalConsumidores * porcentagemRecurso / 100);
    }
    #endregion

    #region AtualizarDisponibilidadesAsync
    public async Task AtualizarDisponibilidadesAsync(string identificadorRecurso, int quantidadeAlvo)
    {
        await _fabricaAlgoritmosAtualizacao.AlgoritmoFisherYates(identificadorRecurso, quantidadeAlvo);
    }
    #endregion
}
