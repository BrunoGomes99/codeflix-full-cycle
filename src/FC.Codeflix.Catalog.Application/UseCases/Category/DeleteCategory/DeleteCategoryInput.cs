using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory
{
    // Aqui, como não vou retornar nada no use case de DeleteCategory,
    // não haverá um output, por isso a herança será apenas IRequest.
    public class DeleteCategoryInput : IRequest
    {
        public Guid Id { get; set; }
        public DeleteCategoryInput(Guid id)
        {
            Id = id;
        }

    }
}
