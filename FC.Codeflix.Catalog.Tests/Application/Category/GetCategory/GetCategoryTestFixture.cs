using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.GetCategory
{
    public class GetCategoryTestFixture : CategoryUseCasesBaseFixture
    {
        // Aqui não tem necessidade de instanciar nem de validar o UnitOfWork nos testes
        // pois no caso do GetCategory não há mudança de estado do agregado Category no banco de dados.
    }

    [CollectionDefinition(nameof(GetCategoryTestFixture))]
    public class GetCategoryTestFixtureCollection : ICollectionFixture<GetCategoryTestFixture>
    {
    }
}
