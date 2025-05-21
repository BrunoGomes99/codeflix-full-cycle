namespace FC.Codeflix.Catalog.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        // Exceções que são referente ao funcionamento da aplicação, ficam na camada de Application, não na de Domain
        public NotFoundException(string? message) : base(message)
        {
        }
    }
}
