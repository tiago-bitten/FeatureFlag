using FeatureFlag.API.Controllers.Infra;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.API.Controllers;

public class RecursosController : ControllerBaseFeatureFlag
{
    #region Ctor
    private readonly IAplicRecurso _aplicRecurso;

    public RecursosController(IAplicRecurso aplicRecurso)
    {
        _aplicRecurso = aplicRecurso;
    }
    #endregion

    #region Adicionar
    [HttpPost("[action]")]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarRecursoRequest request)
    {
        var resposta = await _aplicRecurso.AdicionarAsync(request);
        
        return Sucesso(resposta, "Recurso adicionado com sucesso.");
    }
    #endregion
    
    #region Alterar
    [HttpPut("[action]")]
    public async Task<IActionResult> Alterar([FromBody] AlterarRecursoRequest request)
    {
        var resposta = await _aplicRecurso.AlterarAsync(request);
        
        return Sucesso(resposta, "Recurso alterado com sucesso.");
    }
    #endregion
    
    #region AlterarPorcentagem
    [HttpPut("[action]")]
    public async Task<IActionResult> AlterarPorcentagem([FromBody] AlterarRecursoPorcentagemRequest request)
    {
        var resposta = await _aplicRecurso.AlterarPorcentagemAsync(request);
        
        return Sucesso(resposta, "Porcentagem do recurso alterada com sucesso.");
    }
    #endregion
}