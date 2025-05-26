using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Domain.Exceptions;
using Moq;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("Application", "GetCategory - Use Cases")]
        public async Task GetCategory()
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var exampleCategory = _fixture.GetValidCategory();

            // Aqui, está sendo feito um setup em cima do método Get do repositório,
            // para que, nesse teste, quando o método Get for chamado, e receber como
            // parâmetro um Guid qualquer e um CancellationToken, ele vai retornar automaticamente
            // o exampleCategory que instanciamos acima.
            repositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(exampleCategory);
            var input = new UseCase.GetCategoryInput(exampleCategory.Id);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repository => repository.Get(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

            Assert.NotNull(output);
            Assert.Equal(exampleCategory.Name, output.Name);
            Assert.Equal(exampleCategory.Description, output.Description);
            Assert.Equal(exampleCategory.IsActive, output.IsActive);
            Assert.Equal(exampleCategory.Id, output.Id);
            Assert.Equal(exampleCategory.CreatedAt, output.CreatedAt);
        }

        [Fact(DisplayName = nameof(NotFoundExceptionWhenCategoryDoesntExist))]
        [Trait("Application", "GetCategory - Use Cases")]
        public async Task NotFoundExceptionWhenCategoryDoesntExist()
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var exampleGuid = Guid.NewGuid();
            
            repositoryMock.Setup(x => x.Get(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )).ThrowsAsync(
                new NotFoundException($"Category {exampleGuid} not found.")
            );
            var input = new UseCase.GetCategoryInput(exampleGuid);
            var useCase = new UseCase.GetCategory(repositoryMock.Object);

            // Act
            // Eu espero receber uma task (e não o resultado direto de Handle) pq
            // quero ver o comportamento do método. Quero ver se foi disparada alguma exception
            var task =  async () => await useCase.Handle(input, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(task);

            repositoryMock.Verify(
                repository => repository.Get(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
