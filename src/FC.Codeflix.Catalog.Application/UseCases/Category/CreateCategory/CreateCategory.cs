﻿
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Domain.Repository;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategory : ICreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnityOfWork _unityOfWork;

        public CreateCategory(ICategoryRepository categoryRepository, IUnityOfWork unityOfWork)
        {
            _categoryRepository = categoryRepository;
            _unityOfWork = unityOfWork;
        }

        public async Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
        {
            var category = new DomainEntity.Category(
                input.Name,
                input.Description,
                input.IsActive);

            await _categoryRepository.Insert(category, cancellationToken);
            await _unityOfWork.Commit(cancellationToken);

            return CategoryModelOutput.FromCategory(category);
        }
    }
}
