﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repository;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.GetCategory
{
    public class GetCategory : IGetCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        // Aqui não tem necessidade de passar o UnitOfWork como dependência
        // pois no caso do GetCategory não há mudança de estado do agregado Category no banco de dados.

        public GetCategory(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryModelOutput> Handle(GetCategoryInput request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id, cancellationToken);

            // A verificação se "category" é nulo para retornar ou não uma exception
            // será feita no repository, mas poderia também ser feita aqui no UseCase

            return CategoryModelOutput.FromCategory(category);
        }
    }
}
