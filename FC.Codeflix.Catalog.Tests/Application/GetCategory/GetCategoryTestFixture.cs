using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;

namespace FC.Codeflix.Catalog.UnitTests.Application.GetCategory
{
    public class GetCategoryTestFixture : BaseFixture
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

        public Category GetValidCategory() 
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription()
            );

        public Mock<ICategoryRepository> GetRepositoryMock()
            => new();

        // Aqui não tem necessidade de instanciar nem de validar o UnitOfWork nos testes
        // pois no caso do GetCategory não há mudança de estado do agregado Category no banco de dados.
    }

    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture>
    {
    }
}
