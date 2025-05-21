using FC.Codeflix.Catalog.Domain.Repository;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory
{
    public class GetCategory : IRequestHandler<GetCategoryInput, GetCategoryOutput>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategory(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<GetCategoryOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id, cancellationToken);

            // A verificação se "category" é nulo para retornar ou não uma exception
            // será feita no repository, mas poderia também ser feita aqui no UseCase

            return GetCategoryOutput.FromCategory(category);
        }
    }
}
