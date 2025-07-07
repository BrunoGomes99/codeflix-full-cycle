using FC.Codeflix.Catalog.Application.UseCases.Category.ListCategories;
using Entity = FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.ListCategories
{
    public class ListCategoriesTestFixture : CategoryUseCasesBaseFixture
    {
        public List<Entity.Category> GetExampleCategories(int length = 10)
        {
            var categories = new List<Entity.Category>();
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
    }

    [CollectionDefinition(nameof(ListCategoriesTestFixture))]
    public class ListCategoryTestFixtureCollection : ICollectionFixture<ListCategoriesTestFixture>
    {
    }
}
