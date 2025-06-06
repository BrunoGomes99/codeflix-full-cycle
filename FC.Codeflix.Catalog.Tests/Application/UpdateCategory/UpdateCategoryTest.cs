using Moq;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FC.Codeflix.Catalog.Application.Exceptions;

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

        [Theory(DisplayName = nameof(UpdateCategory))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        [MemberData(
            nameof(UpdateCategoryTestDataGenerator.GetCategoriesToUpdate),
            parameters: 10,
            MemberType = typeof(UpdateCategoryTestDataGenerator)
        )]
        public async Task UpdateCategory(Category exampleCategory, UpdateCategoryInput input)
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var unityOfWorkMock = _fixture.GetUnityOfWorkMock();

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


        [Fact(DisplayName = nameof(ThrowWhenCategoryNotFound))]
        [Trait("Application", "UpdateCategory - Use Cases")]
        public async Task ThrowWhenCategoryNotFound()
        {
            // Arrange
            var repositoryMock = _fixture.GetRepositoryMock();
            var unityOfWorkMock = _fixture.GetUnityOfWorkMock();
            var input = _fixture.GetValidInput();

            repositoryMock.Setup(r => r.Get(
                input.Id,
                It.IsAny<CancellationToken>())
            ).ThrowsAsync(new NotFoundException($"Category {input.Id} not found."));

            var useCase = new UseCase.UpdateCategory(
                repositoryMock.Object,
                unityOfWorkMock.Object
            );

            // Act
            var task = async () => await useCase.Handle(input, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(task);

            repositoryMock.Verify(
                r => r.Get(
                    input.Id,
                    It.IsAny<CancellationToken>()
                ),
                Times.Once
            );
        }
    }
}
