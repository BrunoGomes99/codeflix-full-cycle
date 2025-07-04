namespace FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository
{
    public interface ISearchableRepository<TAggregate> : IRepository where TAggregate : AggregateRoot
    {
        Task<SearchOutput<TAggregate>> Search(
            SearchInput searchInput,
            CancellationToken cancellationToken = default
        );
    }
}
