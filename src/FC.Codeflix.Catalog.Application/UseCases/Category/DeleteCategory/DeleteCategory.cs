using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory
{
    public class DeleteCategory : IDeleteCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnityOfWork _unityOfWork;

        public DeleteCategory(ICategoryRepository categoryRepository, IUnityOfWork unityOfWork)
        {
            _categoryRepository = categoryRepository;
            _unityOfWork = unityOfWork;
        }

        public async Task Handle(DeleteCategoryInput request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id, cancellationToken);
            await _categoryRepository.Delete(category, cancellationToken);
            await _unityOfWork.Commit(cancellationToken);
        }
    }
}
