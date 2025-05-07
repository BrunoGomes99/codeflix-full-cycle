using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using Moq;
using UseCases = FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.CreateCategory
{
    [Collection(nameof(CreateCategoryTestFixture))] // Definição da fixture usada nessa classe de testes
    public class CreateCategoryTest
    {
        private readonly CreateCategoryTestFixture _fixture;

        public CreateCategoryTest(CreateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        // Aqui nos testes da UseCase, o teste não vai ser tão detalhado para cada entrada de Category,
        // por exemplo, nome usado, descrição, data de criação, tamanho de string, range da data etc.
        // Como isso já foi testado na entidade Category, aqui o foco é mais na operação executada pelo
        // useCase, não tanto na entidade em si.

        [Fact(DisplayName = nameof(CreateCategory))]
        [Trait("Application", "CreateCategory - Use Cases")]
        public async void CreateCategory()
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var unityOfWorkMock = _fixture.GetUnityOfWorkMock();

            var useCase = new UseCases.CreateCategory(
                repositoryMock.Object,
                unityOfWorkMock.Object
            );

            var input = _fixture.GetInput();

            // Act
            // A ideia de passar o CancellationToken é uma boa prática de se usar quando chamamos métodos assíncronos.
            // Isso permite propagar esse token ao longo dos métodos chamados pelo principal, tendo um controle caso a
            // operação seja cancelada antes do esperado.
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            // Os dois Assert a seguir verificam se o CategoryRepository e o UnitOfWork chamaram, respectivamente,
            // os métodos Insert e Commit. O repository, verifica se durante o Create foi passados 2 parâmetros,
            // send uma entidade Category e um CancellationToken. Enquanto o unitOfWork, verifica se foi passado
            // um CancellationToken para o Commit. Ambos também checam se essa chamada só aconteceu 1 vez apenas.
            repositoryMock.Verify(
                repository => repository.Insert(
                    It.IsAny<Category>(), 
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

            unityOfWorkMock.Verify(
                unitOfWork => unitOfWork.Commit(
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

            Assert.NotNull(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Description, output.Description);
            Assert.Equal(input.IsActive, output.IsActive);
            Assert.True(output.Id != Guid.Empty);
            Assert.True(output.CreatedAt != default(DateTime));
        }
    }
}
