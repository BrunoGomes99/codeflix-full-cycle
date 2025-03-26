using FC.Codeflix.Catalog.UnitTests.Common;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    // A Fixture é uma classe comum para armazenar elementos de testes de
    // uma mesma entidade, nesse caso categoria, para que evitar repetições
    // de código na classe de testes.
    public class CategoryTestFixture : BaseFixture
    {
        public CategoryTestFixture() : base()
        {
        }

        // Os métodos abaixo garantem que o nome e descrição
        // da categoria seja válido, conforme as regras de negócio
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

        public DomainEntity.Category GetValidCategory() => new (GetValidCategoryName(), GetValidCategoryDescription());
    }

    [CollectionDefinition(nameof(CategoryTestFixture))]
    public class CategoryTextFixtureCollection() : ICollectionFixture<CategoryTestFixture>
    {

    }
}
