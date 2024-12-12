using Microsoft.AspNetCore.Mvc;

namespace FeatureFlag.API.Controllers.Infra;

[ApiController]
[Route("api/[controller]")]
public class ControllerBaseFeatureFlag : ControllerBase
{
    private const string MensagemSucesso = "Operação realizada com sucesso.";

    #region Sucesso
    protected IActionResult Sucesso<T>(T conteudo, string mensagem = MensagemSucesso) where T : class
    {
        var resposta = RespostaBase.Sucesso(conteudo, mensagem);

        return Ok(resposta);
    }
    
    protected IActionResult Sucesso(string mensagem = MensagemSucesso)
    {
        var resposta = RespostaBase.Sucesso(mensagem);

        return Ok(resposta);
    }
    
    protected IActionResult Sucesso<T>(T conteudo, int total, string mensagem = MensagemSucesso) where T : class
    {
        var resposta = RespostaBase.Sucesso(conteudo, total, mensagem);

        return Ok(resposta);
    }
    #endregion
    
    #region Erro
    protected IActionResult Erro(string mensagem)
    {
        var resposta = RespostaBase.Erro(mensagem);

        return BadRequest(resposta);
    }
    #endregion
}