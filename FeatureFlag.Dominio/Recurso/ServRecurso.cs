using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Infra;
using FeatureFlag.Shared.Helpers;

namespace FeatureFlag.Domain;

public class ServRecurso : ServBase<Recurso, IRepRecurso>, IServRecurso
{
    #region Ctor
    private readonly IRepConsumidor _repConsumidor;
    
    public ServRecurso(IRepRecurso repositorio, 
                       IRepConsumidor repConsumidor) 
        : base(repositorio)
    {
        _repConsumidor = repConsumidor;
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
    
    #region AlterarPorcentagemAsync
    public async Task<Recurso> AlterarPorcentagemAsync(Recurso recurso, decimal novaPorcentagem)
    {
        recurso.Porcentagem.Atualizar(novaPorcentagem);
        await AtualizarAsync(recurso);
        
        return recurso;
    }
    #endregion
    
    #region CalcularPorcentagemAsync
    public async Task<decimal> CalcularPorcentagemAsync(Recurso recurso, int? totalConsumidores = null)
    {
        totalConsumidores ??= await _repConsumidor.CountAsync();
        
        return PorcentagemHelper.Calcular(recurso.Consumidor.TotalHabilitados, totalConsumidores.Value);
    }
    #endregion

    #region VerificarPorcentagemAlvoAtingida
    public async Task VerificarPorcentagemAlvoAtingidaAsync(Recurso recurso, int totalConsumidores)
    {
        if (recurso.Consumidor.TotalHabilitados is 0)
        {
            return;
        }
        
        var porcentagemAtualizada = await CalcularPorcentagemAsync(recurso, totalConsumidores);

        if (porcentagemAtualizada > recurso.Porcentagem.Alvo)
        {
            recurso.Porcentagem.Atingir(porcentagemAtualizada);
        }
    }
    #endregion
}