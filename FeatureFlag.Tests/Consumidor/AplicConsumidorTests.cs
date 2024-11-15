using AutoMapper;
using FeatureFlag.Domain;
using FeatureFlag.Aplicacao;
using FeatureFlag.Dominio;
using Moq;

namespace FeatureFlag.Tests;

public class AplicConsumidorTests
{
    private readonly Mock<IServConsumidor> _mockServConsumidor;
    private readonly Mock<IMapper> _mockMapper;
    private readonly AplicRecurso _aplicRecurso;

    public AplicConsumidorTests()
    {
    }
}