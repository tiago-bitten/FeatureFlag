using AutoMapper;
using FeatureFlag.Aplicacao;
using FeatureFlag.Domain;
using FeatureFlag.Domain.Dtos;
using Moq;

namespace FeatureFlag.Tests
{
    public class AplicRecursoTests
    {
        private readonly Mock<IServRecurso> _mockServRecurso;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AplicRecurso _aplicRecurso;

        public AplicRecursoTests()
        {
            _mockServRecurso = new Mock<IServRecurso>();
            _mockMapper = new Mock<IMapper>();
            _aplicRecurso = new AplicRecurso(
                _mockServRecurso.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task AdicionarAsync_CriarRecurso_Sucesso()
        {
            // Arrange
            var request = new CriarRecursoRequest(
                Identificador: "plugzapi",
                Descricao: "Envio de mensagens para o WhatsApp",
                50);

            var recurso = Recurso.Criar("plugzapi", "Envio de mensagens para o WhatsApp", 50);
            
            var response = new RecursoResponse(
                Identificador: "plugzapi",
                Descricao: "Envio de mensagens para o WhatsApp",
                Porcentagem: 0);

            _mockMapper.Setup(m => m.Map<Recurso>(request)).Returns(recurso);
            _mockMapper.Setup(m => m.Map<RecursoResponse>(recurso)).Returns(response);
            _mockServRecurso.Setup(s => s.AdicionarAsync(It.IsAny<Recurso>())).Returns(Task.CompletedTask);

            // Act
            var resultado = await _aplicRecurso.AdicionarAsync(request);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(response.Identificador, resultado.Identificador);
            Assert.Equal(response.Descricao, resultado.Descricao);

            _mockServRecurso.Verify(s => s.AdicionarAsync(It.IsAny<Recurso>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Recurso>(request), Times.Once);
            _mockMapper.Verify(m => m.Map<RecursoResponse>(recurso), Times.Once);
        }

        [Fact]
        public async Task AdicionarAsync_CriarRecursoSemDescricao_DeveLancarErro()
        {
            // Arrange
            var request = new CriarRecursoRequest(
                Identificador: "plugzapi",
                Descricao: string.Empty,
                0); // Descrição vazia

            // Configurando o AutoMapper para mapear CriarRecursoRequest para Recurso,
            // o que chamará o método de criação Recurso.Criar e deve lançar exceção
            _mockMapper.Setup(m => m.Map<Recurso>(request))
                       .Throws(new Exception("Descrição é obrigatória"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _aplicRecurso.AdicionarAsync(request));
            Assert.Equal("Descrição é obrigatória", exception.Message);
        }
    }
}
