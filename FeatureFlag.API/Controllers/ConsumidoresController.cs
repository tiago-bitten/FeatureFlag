using FeatureFlag.API.Controllers.Infra;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.API.Controllers;

public class ConsumidoresController : ControllerBaseFeatureFlag
{
    #region Ctor
    private readonly IAplicConsumidor _aplicConsumidor;

    public ConsumidoresController(IAplicConsumidor aplicConsumidor)
    {
        _aplicConsumidor = aplicConsumidor;
    }
    #endregion
    
    #region Adicionar
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarConsumidorRequest request)
    {
        var resposta = await _aplicConsumidor.AdicionarAsync(request);
        return Sucesso(resposta, "Consumidor adicionado com sucesso.");
    }
    #endregion
    
    #region Alterar
    [HttpPut("{identificador}")]
    public async Task<IActionResult> Alterar([FromBody] AlterarConsumidorRequest request, string identificador)
    {
        var resposta = await _aplicConsumidor.AlterarAsync(identificador, request);
        return Sucesso(resposta, "Consumidor alterado com sucesso.");
    }
    #endregion
    
    #region Remover
    [HttpDelete("{identificador}")]
    public async Task<IActionResult> Remover(string identificador)
    {
        await _aplicConsumidor.RemoverAsync(identificador);
        return Sucesso("Consumidor removido com sucesso.");
    }
    #endregion
}