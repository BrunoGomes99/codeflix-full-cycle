using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;
using Moq;
using FC.Codeflix.Catalog.Application.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Application.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryTest(DeleteCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public async Task DeleteCategory()
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var unityOfWorkMock = _fixture.GetUnityOfWorkMock();
            var categoryExample = _fixture.GetValidCategory();

            // Aqui só preciso fazer o setup pro Get do repository, uma vez que,
            // para deletar, ele primeiro irá chamar o Get.
            // Não faço o setup do Delete, pq ele não vai retornar nada.
            repositoryMock.Setup(
                repository => repository.Get(
                    categoryExample.Id,
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(categoryExample);

            var input = new UseCase.DeleteCategoryInput(categoryExample.Id);

            var useCase = new UseCase.DeleteCategory(
                repositoryMock.Object,
                unityOfWorkMock.Object
            );

            // Act
            await useCase.Handle(input, CancellationToken.None);

            // Assert
            repositoryMock.Verify(
                repository => repository.Get(
                    categoryExample.Id,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

            repositoryMock.Verify(
                repository => repository.Delete(
                    categoryExample,
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
        }

        [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public async Task ThrowWhenCategoryNotFound()
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var unityOfWorkMock = _fixture.GetUnityOfWorkMock();
            var exampleGuid = Guid.NewGuid();

            repositoryMock.Setup(x => x.Get(
                exampleGuid,
                It.IsAny<CancellationToken>()
            )).ThrowsAsync(
                new NotFoundException($"Category '{exampleGuid}' not found.")
            );
            var input = new UseCase.DeleteCategoryInput(exampleGuid);
            var useCase = new UseCase.DeleteCategory(repositoryMock.Object, unityOfWorkMock.Object);

            // Act
            // Eu espero receber uma task pq quero ver o comportamento do método.
            // Quero ver se foi disparada alguma exception
            var task = async () => await useCase.Handle(input, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(task);

            repositoryMock.Verify(
                repository => repository.Get(
                    exampleGuid,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
