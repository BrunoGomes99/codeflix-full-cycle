using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategory
{
    public class ListCategoriesTestFixture : BaseFixture
    {
        public string GetValidCategoryName()
        {
            string categoryName = string.Empty;

            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];

            if (categoryName.Length > 255)
                categoryName = categoryName[..255]; // Esse operador [..255] garante o retorno dos primeiros 255 caracteres da string

            return categoryName;
        }

        public string GetValidCategoryDescription()
        {
            string categoryDescription = Faker.Commerce.ProductDescription();

            if (categoryDescription.Length > 10000)
                categoryDescription = categoryDescription[..10000];

            return categoryDescription;
        }

        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );

        public List<Category> GetExampleCategories(int length = 10)
        {
            var categories = new List<Category>();
            for (int i = 0; i < length; i++)
                categories.Add(GetExampleCategory());
            return categories;
        }

        public ListCategoriesInput GetExampleInput()
        {
            var random = new Random();
            return new(
                page: random.Next(1, 10),
                perPage: random.Next(15, 100),
                search: Faker.Commerce.ProductName(),
                sort: Faker.Commerce.ProductName(),
                sortBy: SearchOrder.Asc
            );
        }

        public Mock<ICategoryRepository> GetRepositoryMock()
            => new();        
    }

    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoryTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture>
    {
    }
}
