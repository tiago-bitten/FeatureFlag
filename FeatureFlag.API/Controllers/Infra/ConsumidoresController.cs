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
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarConsumidorRequest request)
    {
        var resposta = await _aplicConsumidor.AdicionarAsync(request);
        
        return Sucesso(resposta, "Consumidor adicionado com sucesso.");
    }
    #endregion

    #region Alterar
    [HttpPut("{identificador}")]
    public async Task<IActionResult> Alterar(string identificador, [FromBody] AlterarConsumidorRequest request)
    {
        var resposta = await _aplicConsumidor.AlterarAsync(identificador, request);
        return Sucesso(resposta, "Consumidor alterado com sucesso.");
    }
    #endregion

    #region RecuperarPorIdentificador
    [HttpGet("{identificador}")]
    public async Task<IActionResult> RecuperarPorIdentificador(string identificador)
    {
        var resposta = await _aplicConsumidor.RecuperarPorIdentificadorAsync(identificador);
        return Sucesso(resposta, "Consumidor recuperado com sucesso.");
    }
    #endregion
}