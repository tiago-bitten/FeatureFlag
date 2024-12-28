using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public class ServRecurso : ServBase<Recurso, IRepRecurso>, IServRecurso
{
    #region Ctor
    private readonly IRepConsumidor _repConsumidor;
    private readonly IRepRecursoConsumidor _repRecursoConsumidor;
    private readonly IRepControleAcessoConsumidor _repControleAcessoConsumidor;
    
    public ServRecurso(IRepRecurso repositorio, 
                       IRepConsumidor repConsumidor, 
                       IRepRecursoConsumidor repRecursoConsumidor, 
                       IRepControleAcessoConsumidor repControleAcessoConsumidor) 
        : base(repositorio)
    {
        _repConsumidor = repConsumidor;
        _repRecursoConsumidor = repRecursoConsumidor;
        _repControleAcessoConsumidor = repControleAcessoConsumidor;
        _repConsumidor = repConsumidor;
    }
    #endregion

    #region AdicionarAsync
    public override async Task AdicionarAsync(Recurso recurso)
    {
        var identificador = await NomearIdentificadorAsync(recurso.Identificador);
        recurso.AlterarIdentificador(identificador);
        await base.AdicionarAsync(recurso);
    }

    #region NomearIdentificadorAsync
    private async Task<string> NomearIdentificadorAsync(string identificador, int? numeroCopias = 0)
    {
        var existeRecurso = await Repositorio.ExistePorIdentificadorAsync(identificador);
        if (!existeRecurso)
        {
            return identificador;
        }
        numeroCopias++;
        identificador = FormatarCopia(identificador, numeroCopias ?? 0);
        return await NomearIdentificadorAsync(identificador, numeroCopias);

    }
    #endregion
    
    #region FormatarCopia
    private string FormatarCopia(string identificador, int numeroCopias)
    {
        if (identificador.Contains("- Cópia"))
        {
            var index = identificador.LastIndexOf("- Cópia", StringComparison.Ordinal);
            identificador = identificador.Substring(0, index).Trim();
            
            identificador = $"{identificador} - Cópia {numeroCopias}";
            
            return identificador;
        }
        
        return $"{identificador} - Cópia {numeroCopias}";
    }
    #endregion
    #endregion

    #region CalcularPorcentagemAsync
    public async Task<decimal> CalcularPorcentagemAsync(Recurso recurso, int? totalConsumidores = null)
    {
        totalConsumidores ??= await _repConsumidor.CountAsync();
        
        return PorcentagemHelper.Calcular(recurso.Consumidor.TotalHabilitados, totalConsumidores.Value);
    }
    #endregion
    
    #region SincronizarEmbedded
    #region SincronizarConsumidores
    public void SincronizarConsumidores(Recurso recursoAtualizado, List<Consumidor> consumidores)
    {
        foreach (var consumidor in consumidores)
        {
            var consumidorControleAcesso = consumidor.ControleAcessos
                .Where(x => x.Recurso.Id == recursoAtualizado.Id)
                .ToList();

            foreach (var controleAcesso in consumidorControleAcesso)
            {
                controleAcesso.Recurso.Identificador = recursoAtualizado.Identificador;
            }
        }
    }
    #endregion

    #region SincronizarRecursoConsumidores
    public void SincronizarRecursoConsumidores(Recurso recursoAtualizado, List<RecursoConsumidor> recursoConsumidores)
    {
        foreach (var recursoConsumidor in recursoConsumidores)
        {
            recursoConsumidor.Recurso.Identificador = recursoAtualizado.Identificador;
        }
    }
    #endregion

    #region SincronizarControleAcessoConsumidores
    public void SincronizarControleAcessoConsumidores(Recurso recursoAtualizado, List<ControleAcessoConsumidor> controleAcessoConsumidores)
    {
        foreach (var controleAcessoConsumidor in controleAcessoConsumidores)
        {
            controleAcessoConsumidor.Recurso.Identificador = recursoAtualizado.Identificador;
        }
    }
    #endregion
    #endregion
}