using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.API.Controllers.Infra;

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
    [HttpPost("[action]")]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarConsumidorRequest request)
    {
        var resposta = await _aplicConsumidor.AdicionarAsync(request);
        
        return Sucesso(resposta, "Consumidor adicionado com sucesso.");
    }
    #endregion
}