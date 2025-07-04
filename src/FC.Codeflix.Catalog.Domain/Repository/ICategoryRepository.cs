using FC.Codeflix.Catalog.Domain.Entity;
using FC.Codeflix.Catalog.Domain.SeedWork;
using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace FC.Codeflix.Catalog.Domain.Repository
{
    // Os contratos do repositório (interfaces) também fazem parte do Domínio, uma vez que descrevem regras de negócio também
    // Notas sobre Repositórios (Colocar esse comentário na classe repositório de fato):
    //  - O DDD trabalha como um repositório para cada raiz de agregação(não necessariamente entidades)
    //  - O repositório não deve transparecer operações no banco em si, como queries, qual o tipo de banco está usando etc.
    //  - O ideal é que o repositório pareça apenas uma lista em memória que responsável por persistir os dados

    public interface ICategoryRepository : IGenericRepository<Category>, ISearchableRepository<Category>
    {        
    }
}