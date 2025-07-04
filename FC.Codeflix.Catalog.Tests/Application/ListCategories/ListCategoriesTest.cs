using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using UseCase = FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using Moq;
using FC.Codeflix.Catalog.UnitTests.Application.ListCategories;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategory
{
    [Collection(nameof(ListCategoriesTestFixture))]
    public class ListCategoriesTest
    {
        private readonly ListCategoriesTestFixture _fixture;

        public ListCategoriesTest(ListCategoriesTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(List))]
        [Trait("Application", "ListCategories - Use Cases")]
        public async Task List()
        {
            // Arrange
            var categoriesExample = _fixture.GetExampleCategories();
            var repositoryMock = _fixture.GetRepositoryMock();
            var input = _fixture.GetExampleInput();

            var outputRepositoryCategory = new SearchOutput<Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<Category>)categoriesExample,
                total: (new Random()).Next(50, 200)
            );

            // Aqui haverá dois inputs diferentes. Um para o use case de ListCategories e outro para o repositório (Search Input)
            // Apesar de parecidos, os dois terão funções diferentes, onde o ListCategoriesInput será usada para exibição das categorias,
            // enquanto que o SearchInput será usada para uma busca no banco, via repositório, de forma mais genérica.

            // Mesma coisa na saída, a classe SearchOutput será genérica para qualquer busca de agragado no repositório.

            // No mock, é especificado que não é qualquer SearchInput que vai retornar o que desejamos,
            // e sim um SearchInput cujos parâmetros tenham os valores do input (ListCategoriesInput)
            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.SortBy
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositoryCategory);

            var useCase = new UseCase.ListCategories(repositoryMock.Object);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            
            // Primeiro compara a lista retornada em si
            Assert.NotNull(output);
            Assert.Equal(outputRepositoryCategory.CurrentPage, output.Page);
            Assert.Equal(outputRepositoryCategory.PerPage, output.PerPage);
            Assert.Equal(outputRepositoryCategory.Total, output.Total);
            Assert.True(outputRepositoryCategory.Items.Count == output.Items.Count);

            // Depois percorre toda a lista, validando todos os itens da lista.
            // A lista em questão será um List<CategoryModelOutput>,
            ((List<CategoryModelOutput>) output.Items).ForEach(outputItem =>
            {
                var repositoryCategory = categoriesExample.Find(c => c.Id == outputItem.Id);
                Assert.NotNull(repositoryCategory);
                Assert.Equal(repositoryCategory.Name, outputItem.Name);
                Assert.Equal(repositoryCategory.Description, outputItem.Description);
                Assert.Equal(repositoryCategory.IsActive, outputItem.IsActive);
                Assert.Equal(repositoryCategory.CreatedAt, outputItem.CreatedAt);
            });

            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.SortBy
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }


        [Theory(DisplayName = nameof(ListInputWithoutAllParameters))]
        [Trait("Application", "ListCategories - Use Cases")]
        [MemberData(
            nameof(ListCategoriesTestDataGenerator.GetInputsWithoutAllParameters),
            parameters: 12,
            MemberType = typeof(ListCategoriesTestDataGenerator)
        )]
        public async Task ListInputWithoutAllParameters(UseCase.ListCategoriesInput input)
        {
            // Arrange
            var categoriesExample = _fixture.GetExampleCategories();
            var repositoryMock = _fixture.GetRepositoryMock();

            var outputRepositoryCategory = new SearchOutput<Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (IReadOnlyList<Category>)categoriesExample,
                total: (new Random()).Next(50, 200)
            );

            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.SortBy
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositoryCategory);

            var useCase = new UseCase.ListCategories(repositoryMock.Object);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            Assert.NotNull(output);
            Assert.Equal(outputRepositoryCategory.CurrentPage, output.Page);
            Assert.Equal(outputRepositoryCategory.PerPage, output.PerPage);
            Assert.Equal(outputRepositoryCategory.Total, output.Total);
            Assert.True(outputRepositoryCategory.Items.Count == output.Items.Count);

            ((List<CategoryModelOutput>)output.Items).ForEach(outputItem =>
            {
                var repositoryCategory = categoriesExample.Find(c => c.Id == outputItem.Id);
                Assert.NotNull(repositoryCategory);
                Assert.Equal(repositoryCategory.Name, outputItem.Name);
                Assert.Equal(repositoryCategory.Description, outputItem.Description);
                Assert.Equal(repositoryCategory.IsActive, outputItem.IsActive);
                Assert.Equal(repositoryCategory.CreatedAt, outputItem.CreatedAt);
            });

            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.SortBy
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }


        [Fact(DisplayName = nameof(ListOkWhenEmpty))]
        [Trait("Application", "ListCategories - Use Cases")]
        public async Task ListOkWhenEmpty()
        {
            // Arrange
            var input = _fixture.GetExampleInput();
            var repositoryMock = _fixture.GetRepositoryMock();

            var outputRepositoryCategory = new SearchOutput<Category>(
                currentPage: input.Page,
                perPage: input.PerPage,
                items: (new List<Category>()).AsReadOnly(),
                total: 0
            );

            repositoryMock.Setup(x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.SortBy
                ),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(outputRepositoryCategory);

            var useCase = new UseCase.ListCategories(repositoryMock.Object);

            // Act
            var output = await useCase.Handle(input, CancellationToken.None);

            // Assert
            Assert.NotNull(output);
            Assert.Equal(outputRepositoryCategory.CurrentPage, output.Page);
            Assert.Equal(outputRepositoryCategory.PerPage, output.PerPage);
            Assert.Equal(0, output.Total);
            Assert.True(output.Items.Count == 0);

            repositoryMock.Verify(x => x.Search(
                It.Is<SearchInput>(searchInput =>
                    searchInput.Page == input.Page &&
                    searchInput.PerPage == input.PerPage &&
                    searchInput.Search == input.Search &&
                    searchInput.OrderBy == input.Sort &&
                    searchInput.Order == input.SortBy
                ),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }
    }
}
