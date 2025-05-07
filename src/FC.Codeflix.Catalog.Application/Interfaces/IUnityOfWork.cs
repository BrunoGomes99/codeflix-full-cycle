namespace FC.Codeflix.Catalog.Application.Interfaces
{
    // Notas sobre UnitOfWorks (Colocar esse comentário na classe UnitOfWork de fato):
    // Possui a função de reduzir a quantidade de acessos ao banco. Ele persiste todas as alterações dos agregados em memória,
    // e só depois faz um commit uma vez para realizar todas as operações de banco uma vez apenas.
    // Ele também trabalha com transações.Dessa forma, se alguma operação der errado, ele consegue fazer um rollback antes do commit.
    public interface IUnityOfWork
    {
        public Task Commit(CancellationToken cancellationToken);
    }
}
