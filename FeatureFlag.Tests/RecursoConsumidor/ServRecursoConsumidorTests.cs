using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using Moq;

namespace FeatureFlag.Tests;

public class ServRecursoConsumidorTests
{
    #region Ctor
    private readonly Mock<IRepRecursoConsumidor> _mockRepRecursoConsumidor;
    private readonly Mock<IRepRecurso> _mockRepRecurso;
    private readonly Mock<IRepConsumidor> _mockRepConsumidor;
    private readonly Mock<IRepControleAcessoConsumidor> _mockControlAcessoConsumidor;
    private readonly ServRecursoConsumidor _servRecursoConsumidor;

    public ServRecursoConsumidorTests()
    {
        _mockRepRecursoConsumidor = new Mock<IRepRecursoConsumidor>();
        _mockRepRecurso = new Mock<IRepRecurso>();
        _mockRepConsumidor = new Mock<IRepConsumidor>();
        _mockControlAcessoConsumidor = new Mock<IRepControleAcessoConsumidor>();

        _servRecursoConsumidor = new ServRecursoConsumidor(
            _mockRepRecursoConsumidor.Object,
            _mockRepRecurso.Object,
            _mockRepConsumidor.Object,
            _mockControlAcessoConsumidor.Object);
    }
    #endregion

    #region Utils
    private List<RecursoConsumidor> CriarRecursoConsumidores(int quantidade, EnumStatusRecursoConsumidor status)
    {
        return Enumerable.Range(1, quantidade)
            .Select(_ =>
            {
                var recursoConsumidor = RecursoConsumidor.Criar(Guid.NewGuid(), Guid.NewGuid());
                if (status == EnumStatusRecursoConsumidor.Habilitado)
                    recursoConsumidor.Habilitar();
                else
                    recursoConsumidor.Desabilitar();

                return recursoConsumidor;
            })
            .ToList();
    }
    #endregion

    #region CalcularQuantidadeParaHabilitarAsync

    #region TotalConsumidoresZero_DeveRetornarZero
    [Fact]
    public async Task CalcularQuantidadeParaHabilitarAsync_TotalConsumidoresZero_DeveRetornarZero()
    {
        // Arrange
        _mockRepRecurso.Setup(x => x.RecuperarPorcentagemPorIdentificadorAsync("plugzapi")).ReturnsAsync(50);
        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(0);

        // Act
        var resultado = await _servRecursoConsumidor.CalcularQuantidadeParaHabilitarAsync("plugzapi");

        // Assert
        Assert.Equal(0, resultado);
    }
    #endregion

    #region PorcentagemZero_DeveRetornarZero
    [Fact]
    public async Task CalcularQuantidadeParaHabilitarAsync_PorcentagemZero_DeveRetornarZero()
    {
        // Arrange
        _mockRepRecurso.Setup(x => x.RecuperarPorcentagemPorIdentificadorAsync("plugzapi")).ReturnsAsync(0);
        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(100);

        // Act
        var resultado = await _servRecursoConsumidor.CalcularQuantidadeParaHabilitarAsync("plugzapi");

        // Assert
        Assert.Equal(0, resultado);
    }
    #endregion

    #region PorcentagemCinquenta_TotalDois_DeveRetornarUm
    [Fact]
    public async Task CalcularQuantidadeParaHabilitarAsync_PorcentagemCinquenta_TotalDois_DeveRetornarUm()
    {
        // Arrange
        _mockRepRecurso.Setup(x => x.RecuperarPorcentagemPorIdentificadorAsync("plugzapi")).ReturnsAsync(50);
        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(2);

        // Act
        var resultado = await _servRecursoConsumidor.CalcularQuantidadeParaHabilitarAsync("plugzapi");

        // Assert
        Assert.Equal(1, resultado);
    }
    #endregion

    #region PorcentagemVinteCinco_TotalQuatro_DeveRetornarUm
    [Fact]
    public async Task CalcularQuantidadeParaHabilitarAsync_PorcentagemVinteCinco_TotalQuatro_DeveRetornarUm()
    {
        // Arrange
        _mockRepRecurso.Setup(x => x.RecuperarPorcentagemPorIdentificadorAsync("plugzapi")).ReturnsAsync(25);
        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(4);

        // Act
        var resultado = await _servRecursoConsumidor.CalcularQuantidadeParaHabilitarAsync("plugzapi");

        // Assert
        Assert.Equal(1, resultado);
    }
    #endregion

    #region PorcentagemTrinta_TotalDez_DeveRetornarTres
    [Fact]
    public async Task CalcularQuantidadeParaHabilitarAsync_PorcentagemTrinta_TotalDez_DeveRetornarTres()
    {
        // Arrange
        _mockRepRecurso.Setup(x => x.RecuperarPorcentagemPorIdentificadorAsync("plugzapi")).ReturnsAsync(30);
        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(10);

        // Act
        var resultado = await _servRecursoConsumidor.CalcularQuantidadeParaHabilitarAsync("plugzapi");

        // Assert
        Assert.Equal(3, resultado);
    }
    #endregion

    #endregion

    #region AtualizarDisponibilidadesAsync

    #region QuantidadeJaAtingida_NaoDeveAlterarHabilitados
    [Fact]
    public async Task AtualizarDisponibilidadesAsync_QuantidadeJaAtingida_NaoDeveAlterarHabilitados()
    {
        // Arrange
        var identificadorRecurso = "plugzapi";
        var quantidadeAlvo = 5;
        
        var habilitados = CriarRecursoConsumidores(quantidadeAlvo, EnumStatusRecursoConsumidor.Habilitado);
        var desabilitados = new List<RecursoConsumidor>();

        _mockRepRecursoConsumidor.Setup(x => x.RecuperarHabilitadosPorRecurso(identificadorRecurso)).Returns(habilitados.AsQueryable);
        _mockRepRecursoConsumidor.Setup(x => x.RecuperarDesabilitadosPorRecurso(identificadorRecurso)).Returns(desabilitados.AsQueryable);

        // Act
        await _servRecursoConsumidor.AtualizarDisponibilidadesAsync(identificadorRecurso, quantidadeAlvo);

        // Assert
        _mockRepRecursoConsumidor.Verify(x => x.RecuperarHabilitadosPorRecurso(identificadorRecurso), Times.Once);
        _mockRepRecursoConsumidor.Verify(x => x.RecuperarDesabilitadosPorRecurso(identificadorRecurso), Times.Once);
        Assert.All(habilitados, x => Assert.Equal(EnumStatusRecursoConsumidor.Habilitado, x.Status));
    }
    #endregion

    #region MenosHabilitadosQueAlvo_DeveHabilitarConsumidores
    [Fact]
    public async Task AtualizarDisponibilidadesAsync_MenosHabilitadosQueAlvo_DeveHabilitarConsumidores()
    {
        // Arrange
        var identificadorRecurso = "plugzapi";
        var quantidadeAlvo = 5;

        var habilitados = CriarRecursoConsumidores(3, EnumStatusRecursoConsumidor.Habilitado);
        var desabilitados = CriarRecursoConsumidores(3, EnumStatusRecursoConsumidor.Desabilitado);

        _mockRepRecursoConsumidor.Setup(x => x.RecuperarHabilitadosPorRecurso(identificadorRecurso)).Returns(habilitados.AsQueryable);
        _mockRepRecursoConsumidor.Setup(x => x.RecuperarDesabilitadosPorRecurso(identificadorRecurso)).Returns(desabilitados.AsQueryable);

        // Act
        await _servRecursoConsumidor.AtualizarDisponibilidadesAsync(identificadorRecurso, quantidadeAlvo);

        // Assert
        var totalHabilitados = habilitados.Concat(desabilitados).Count(x => x.Status == EnumStatusRecursoConsumidor.Habilitado);
        Assert.Equal(quantidadeAlvo, totalHabilitados);
    }
    #endregion

    #region MaisHabilitadosQueAlvo_DeveDesabilitarConsumidores
    [Fact]
    public async Task AtualizarDisponibilidadesAsync_MaisHabilitadosQueAlvo_DeveDesabilitarConsumidores()
    {
        // Arrange
        var identificadorRecurso = "plugzapi";
        var quantidadeAlvo = 3;

        var habilitados = CriarRecursoConsumidores(5, EnumStatusRecursoConsumidor.Habilitado);
        var desabilitados = CriarRecursoConsumidores(1, EnumStatusRecursoConsumidor.Desabilitado);

        _mockRepRecursoConsumidor.Setup(x => x.RecuperarHabilitadosPorRecurso(identificadorRecurso)).Returns(habilitados.AsQueryable);
        _mockRepRecursoConsumidor.Setup(x => x.RecuperarDesabilitadosPorRecurso(identificadorRecurso)).Returns(desabilitados.AsQueryable);

        // Act
        await _servRecursoConsumidor.AtualizarDisponibilidadesAsync(identificadorRecurso, quantidadeAlvo);

        // Assert
        var totalHabilitados = habilitados.Concat(desabilitados).Count(x => x.Status == EnumStatusRecursoConsumidor.Habilitado);
        Assert.Equal(quantidadeAlvo, totalHabilitados);
    }
    #endregion

    #endregion
}
