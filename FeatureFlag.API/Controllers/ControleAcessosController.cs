using FeatureFlag.API.Controllers.Infra;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.API.Controllers;

public class ControleAcessosController : ControllerBaseFeatureFlag
{
    #region Ctor
    private readonly IAplicControleAcessoConsumidor _aplicControleAcessoConsumidor;

    public ControleAcessosController(IAplicControleAcessoConsumidor aplicControleAcessoConsumidor)
    {
        _aplicControleAcessoConsumidor = aplicControleAcessoConsumidor;
    }
    #endregion
    
    #region Adicionar
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarControleAcessoConsumidorRequest request)
    {
        var resposta = await _aplicControleAcessoConsumidor.AdicionarAsync(request);
        return Sucesso(resposta, "Controle de acesso adicionado com sucesso.");
    }
    #endregion
    
    #region RecuperarPorConsumidor
    [HttpGet("Consumidor/{codigoConsumidor:int}")]
    public async Task<IActionResult> RecuperarPorConsumidor(int codigoConsumidor)
    {
        var resposta = await _aplicControleAcessoConsumidor.RecuperarPorConsumidorAsync(codigoConsumidor);
        return Sucesso(resposta, resposta.Count, "Controles de acesso recuperados com sucesso.");
    }
    #endregion
}