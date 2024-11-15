using Moq;
using FeatureFlag.Aplicacao;
using FeatureFlag.Dominio.RecursoConsumidor.Dtos;
using FeatureFlag.Domain;
using AutoMapper;

namespace FeatureFlag.Tests;

public class AplicRecursoConsumidorTests
{
    private readonly Mock<IServRecursoConsumidor> _mockServRecursoConsumidor;
    private readonly Mock<IServConsumidor> _mockServConsumidor;
    private readonly Mock<IServRecurso> _mockServRecurso;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AplicRecursoConsumidor _aplicRecursoConsumidor;

    public AplicRecursoConsumidorTests()
    {
        _mockServRecursoConsumidor = new Mock<IServRecursoConsumidor>();
        _mockServConsumidor = new Mock<IServConsumidor>();
        _mockServRecurso = new Mock<IServRecurso>();
        _mockMapper = new Mock<IMapper>();

        _aplicRecursoConsumidor = new AplicRecursoConsumidor(
            _mockServRecursoConsumidor.Object,
            _mockServConsumidor.Object,
            _mockServRecurso.Object,
            _mockMapper.Object
        );
    }

    [Fact]
    public async Task RecuperarPorRecursoConsumidorAsync_ConsumidorNaoExistente_DeveCriarConsumidor()
    {
        // Arrange
        var param = new RecuperarPorRecursoConsumidorParam(
            IdentificadorRecurso: "plugzapi",
            IdentificadorConsumidor: "UN_13");
                
        // Mock para retornar nulo (consumidor não existente)
        _mockServRecursoConsumidor.Setup(x => x.Repositorio.RecuperarPorRecursoConsumidorAsync(
                param.IdentificadorRecurso, param.IdentificadorRecurso))
            .ReturnsAsync((RecursoConsumidorResponse?)null);

        // Configuração para Adicionar Consumidor
        _mockServConsumidor.Setup(x => x.AdicionarAsync(It.IsAny<Consumidor>()))
            .Returns(Task.CompletedTask);

        // Act
        var resultado = await _aplicRecursoConsumidor.RecuperarPorRecursoConsumidorAsync(param);

        // Assert
        _mockServConsumidor.Verify(x => x.AdicionarAsync(It.IsAny<Consumidor>()), Times.Once);
        Assert.Equal(param.IdentificadorConsumidor, resultado.Consumidor);
        Assert.False(resultado.Habilitado); // Deve ser falso para um consumidor recém-criado
    }

    [Fact]
    public async Task RecuperarPorRecursoConsumidorAsync_ConsumidorExistente_DeveRetornarRecursoConsumidor()
    {
        // Arrange
        var param = new RecuperarPorRecursoConsumidorParam(
            IdentificadorRecurso: "plugzapi",
            IdentificadorConsumidor: "UN_13");

        var recursoConsumidorResponse = new RecursoConsumidorResponse("plugzapi", "UN_13", true);

        _mockServRecursoConsumidor.Setup(x => x.Repositorio.RecuperarPorRecursoConsumidorAsync(
                param.IdentificadorRecurso, param.IdentificadorRecurso))
            .ReturnsAsync(recursoConsumidorResponse);

        // Act
        var resultado = await _aplicRecursoConsumidor.RecuperarPorRecursoConsumidorAsync(param);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal("UN_13", resultado.Consumidor);
        Assert.True(resultado.Habilitado); // Deve retornar como habilitado
    }

    [Fact]
    public async Task RecuperarPorConsumidorAsync_ConsumidorComRecursos_DeveRetornarRecursos()
    {
        // Arrange
        var param = new RecuperarPorConsumidorParam(
            IdentificadorConsumidor: "UN_13");

        var recursosConsumidor = new List<RecursoConsumidorResponse>
        {
            new("plugzapi", "UN_13", true),
            new("treino", "UN_13", false)
        };

        _mockServRecursoConsumidor.Setup(x => x.Repositorio.RecuperarPorConsumidor(param.IdentificadorConsumidor))
            .Returns(recursosConsumidor.AsQueryable());

        // Act
        var resultado = await _aplicRecursoConsumidor.RecuperarPorConsumidorAsync(param);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("plugzapi", resultado[0].Recurso);
        Assert.True(resultado[0].Habilitado);
    }
}