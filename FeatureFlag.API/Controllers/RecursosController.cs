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
    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarRecursoRequest request)
    {
        var resposta = await _aplicRecurso.AdicionarAsync(request);
        
        return Sucesso(resposta, "Recurso adicionado com sucesso.");
    }
    #endregion
    
    #region Alterar
    [HttpPut("{identificador}")]
    public async Task<IActionResult> Alterar([FromBody] AlterarRecursoRequest request, string identificador)
    {
        var resposta = await _aplicRecurso.AlterarAsync(identificador, request);
        
        return Sucesso(resposta, "Recurso alterado com sucesso.");
    }
    #endregion
    
    #region RecuperarTodos
    [HttpGet]
    public async Task<IActionResult> RecuperarTodos()
    {
        var resposta = await _aplicRecurso.RecuperarTodosAsync();
        
        return Sucesso(resposta, resposta.Count, "Recursos recuperados com sucesso.");
    }
    #endregion
    
    #region RecuperarPorIdentificador
    [HttpGet("{identificador}")]
    public async Task<IActionResult> RecuperarPorIdentificador(string identificador)
    {
        var resposta = await _aplicRecurso.RecuperarPorIdentificadorAsync(identificador);
        return Sucesso(resposta, "Recurso recuperado com sucesso.");
    }
    #endregion
}