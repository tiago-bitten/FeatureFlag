using FeatureFlag.Domain;
using FeatureFlag.Dominio;
using FeatureFlag.Tests.Utils;
using Moq;

namespace FeatureFlag.Tests;

public class ServRecursoConsumidorTests
{
    #region Ctor
    private readonly Mock<IRepRecursoConsumidor> _mockRepositorio;
    private readonly Mock<IRepRecurso> _mockRepRecurso;
    private readonly Mock<IRepConsumidor> _mockRepConsumidor;
    private readonly Mock<IRepControleAcessoConsumidor> _mockRepControleAcessoConsumidor;
    private readonly ServRecursoConsumidor _servico;

    public ServRecursoConsumidorTests()
    {
        _mockRepositorio = new Mock<IRepRecursoConsumidor>();
        _mockRepRecurso = new Mock<IRepRecurso>();
        _mockRepConsumidor = new Mock<IRepConsumidor>();
        _mockRepControleAcessoConsumidor = new Mock<IRepControleAcessoConsumidor>();

        _servico = new ServRecursoConsumidor(
            _mockRepositorio.Object,
            _mockRepRecurso.Object,
            _mockRepConsumidor.Object,
            _mockRepControleAcessoConsumidor.Object
        );
    }
    #endregion

    #region AtualizarStatusAsync | RecursoConsumidor não persistido

    #region AtualizarStatusAsync_50PorcentoAlvo_AtualZero_DeveHabilitarPrimeiro
    /// <summary>
    /// Porcentagem alvo é 50%
    /// Porcentagem atual é 0%
    /// RecursoConsumidor não está persistido
    /// Status do RecursoConsumidor deve ser Habilitado
    /// </summary>
    [Fact]
    public async Task AtualizarStatusAsync_50PorcentoAlvo_AtualZero_DeveHabilitarPrimeiro()
    {
        // Arrange
        var recurso = RecursoConsumidorTestUtils.CriarRecurso("plugzapi", "Envio automático");
        var recursoConsumidor = RecursoConsumidorTestUtils.CriarRecursoConsumidor(recurso);

        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(1);
        _mockRepositorio.Setup(x => x.RecuperarPorStatus(EnumStatusRecursoConsumidor.Habilitado))
            .Returns(new List<RecursoConsumidor>().AsQueryable());

        // Act
        await _servico.AtualizarStatusAsync(recursoConsumidor);

        // Assert
        Assert.Equal(EnumStatusRecursoConsumidor.Habilitado, recursoConsumidor.Status);
    }
    #endregion

    #region AtualizarStatusAsync_50PorcentoAlvo_Atual50_DeveDesabilitarSegundoConsumidor
    /// <summary>
    /// Porcentagem alvo é 50%
    /// Porcentagem atual é 50%
    /// RecursoConsumidor não está persistido
    /// Status do RecursoConsumidor deve ser Desabilitado
    /// </summary>
    [Fact]
    public async Task AtualizarStatusAsync_50PorcentoAlvo_Atual50_DeveDesabilitarSegundoConsumidor()
    {
        // Arrange
        var recurso = RecursoConsumidorTestUtils.CriarRecurso("plugzapi", "Envio automático");
        var recursoConsumidor = RecursoConsumidorTestUtils.CriarRecursoConsumidor(recurso);

        var recursosHabilitados = RecursoConsumidorTestUtils.CriarListaRecursoConsumidores(
            quantidade: 1,
            status: EnumStatusRecursoConsumidor.Habilitado,
            recurso: recurso);

        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(2);
        _mockRepositorio.Setup(x => x.RecuperarPorStatus(EnumStatusRecursoConsumidor.Habilitado))
            .Returns(recursosHabilitados.AsQueryable());

        // Act
        await _servico.AtualizarStatusAsync(recursoConsumidor);

        // Assert
        Assert.Equal(EnumStatusRecursoConsumidor.Desabilitado, recursoConsumidor.Status);
    }
    #endregion

    #region AtualizarStatusAsync_50PorcentoAlvo_Atual33_DeveHabilitarTerceiroConsumidor
    /// <summary>
    /// Porcentagem alvo é 50%
    /// Porcentagem atual é 33%
    /// RecursoConsumidor não está persistido
    /// Status do RecursoConsumidor deve ser Habilitado
    /// </summary>
    [Fact]
    public async Task AtualizarStatusAsync_50PorcentoAlvo_Atual33_DeveHabilitarTerceiroConsumidor()
    {
        // Arrange
        var recurso = RecursoConsumidorTestUtils.CriarRecurso("plugzapi", "Envio automático");
        var recursoConsumidor = RecursoConsumidorTestUtils.CriarRecursoConsumidor(recurso);

        var recursosHabilitados = RecursoConsumidorTestUtils.CriarListaRecursoConsumidores(
            quantidade: 1,
            status: EnumStatusRecursoConsumidor.Habilitado,
            recurso: recurso);

        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(3);
        _mockRepositorio.Setup(x => x.RecuperarPorStatus(EnumStatusRecursoConsumidor.Habilitado))
            .Returns(recursosHabilitados.AsQueryable());

        // Act
        await _servico.AtualizarStatusAsync(recursoConsumidor);

        // Assert
        Assert.Equal(EnumStatusRecursoConsumidor.Habilitado, recursoConsumidor.Status);
    }
    #endregion

    #endregion

    #region AtualizarStatusAsync | RecursoConsumidor persistido

    #region AtualizarStatusAsync_50PorcentoAlvo_Atual50_RecursoHabilitado_NaoDeveAlterarStatus
    /// <summary>
    /// Porcentagem alvo é 50%
    /// Porcentagem atual é 50%
    /// RecursoConsumidor já está persistido com status Habilitado
    /// Status do RecursoConsumidor não deve mudar
    /// </summary>
    [Fact]
    public async Task AtualizarStatusAsync_50PorcentoAlvo_Atual50_RecursoHabilitado_NaoDeveAlterarStatus()
    {
        // Arrange
        var recurso = RecursoConsumidorTestUtils.CriarRecurso("plugzapi", "Envio automático");
        var recursoConsumidor = RecursoConsumidorTestUtils.CriarRecursoConsumidor(recurso, status: EnumStatusRecursoConsumidor.Habilitado);

        var recursosHabilitados = RecursoConsumidorTestUtils.CriarListaRecursoConsumidores(
            quantidade: 1,
            status: EnumStatusRecursoConsumidor.Habilitado,
            recurso: recurso);

        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(2);
        _mockRepositorio.Setup(x => x.RecuperarPorStatus(EnumStatusRecursoConsumidor.Habilitado))
            .Returns(recursosHabilitados.AsQueryable());

        // Act
        await _servico.AtualizarStatusAsync(recursoConsumidor);

        // Assert
        Assert.Equal(EnumStatusRecursoConsumidor.Habilitado, recursoConsumidor.Status);
    }
    #endregion

    #region AtualizarStatusAsync_50PorcentoAlvo_Atual66_RecursoDesabilitado_NaoDeveAlterarStatus
    /// <summary>
    /// Porcentagem alvo é 50%
    /// Porcentagem atual é 66%
    /// RecursoConsumidor já está persistido com status Desabilitado
    /// Status do RecursoConsumidor não deve mudar
    /// </summary>
    [Fact]
    public async Task AtualizarStatusAsync_50PorcentoAlvo_Atual66_RecursoDesabilitado_NaoDeveAlterarStatus()
    {
        // Arrange
        var recurso = RecursoConsumidorTestUtils.CriarRecurso("plugzapi", "Envio automático");
        var recursoConsumidor = RecursoConsumidorTestUtils.CriarRecursoConsumidor(recurso, status: EnumStatusRecursoConsumidor.Desabilitado);

        var recursosHabilitados = RecursoConsumidorTestUtils.CriarListaRecursoConsumidores(
            quantidade: 2,
            status: EnumStatusRecursoConsumidor.Habilitado,
            recurso: recurso);

        _mockRepConsumidor.Setup(x => x.CountAsync()).ReturnsAsync(3);
        _mockRepositorio.Setup(x => x.RecuperarPorStatus(EnumStatusRecursoConsumidor.Habilitado))
            .Returns(recursosHabilitados.AsQueryable());

        // Act
        await _servico.AtualizarStatusAsync(recursoConsumidor);

        // Assert
        Assert.Equal(EnumStatusRecursoConsumidor.Desabilitado, recursoConsumidor.Status);
    }
    #endregion

    #endregion
}
