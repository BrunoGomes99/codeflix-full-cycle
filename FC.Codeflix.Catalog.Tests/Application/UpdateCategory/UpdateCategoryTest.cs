using Moq;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        public async Task UpdateCategory()
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var unityOfWorkMock = _fixture.GetUnityOfWorkMock();

            var exampleCategory = _fixture.GetExampleCategory();
            var input = new UseCase.UpdateCategoryInput(
                exampleCategory.Id,
                _fixture.GetValidCategoryName(),
                _fixture.GetValidCategoryDescription(),
                !exampleCategory.IsActive
            );

            repositoryMock.Setup(r => r.Get(
                exampleCategory.Id,
                It.IsAny<CancellationToken>())
            ).ReturnsAsync(exampleCategory);

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unityOfWorkMock.Object
            );

            // Act
            CategoryModelOutput output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            Assert.NotNull(output);
            Assert.Equal(input.Name, output.Name);
            Assert.Equal(input.Description, output.Description);
            Assert.Equal(input.IsActive, output.IsActive);
            Assert.Equal(input.Id, output.Id);

            repositoryMock.Verify(
                r => r.Get(
                    exampleCategory.Id,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

            repositoryMock.Verify(
                r => r.Update(
                    exampleCategory,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );

            unityOfWorkMock.Verify(
                u => u.Commit(
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
