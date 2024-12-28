using FeatureFlag.API.Controllers.Infra;
using FeatureFlag.Dominio;
using FeatureFlag.Dominio.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.API.Controllers;

public class RecursosConsumidoresController : ControllerBaseFeatureFlag
{
    #region Ctor
    private readonly IAplicRecursoConsumidor _aplicRecursoConsumidor;
    
    public RecursosConsumidoresController(IAplicRecursoConsumidor aplicRecursoConsumidor)
    {
        _aplicRecursoConsumidor = aplicRecursoConsumidor;
    }
    #endregion
    
    #region RecuperarPorRecursoConsumidor
    [HttpGet("[action]")]
    public async Task<IActionResult> RecuperarPorRecursoConsumidor([FromQuery] RecuperarPorRecursoConsumidorParam param)
    {
        var resposta = await _aplicRecursoConsumidor.RecuperarPorRecursoConsumidorAsync(param);
        
        return Sucesso(resposta, "Recursos e consumidores recuperados com sucesso.");
    }
    #endregion
}