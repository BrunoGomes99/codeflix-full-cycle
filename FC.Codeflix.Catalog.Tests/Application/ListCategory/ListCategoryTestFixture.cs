using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.ListCategory
{
    public class ListCategoryTestFixture : BaseFixture
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

        public Mock<ICategoryRepository> GetRepositoryMock()
            => new();        
    }

    [CollectionDefinition(nameof(ListCategoryTestFixture))]
    public class ListCategoryTestFixtureCollection : ICollectionFixture<ListCategoryTestFixture>
    {
    }
}
