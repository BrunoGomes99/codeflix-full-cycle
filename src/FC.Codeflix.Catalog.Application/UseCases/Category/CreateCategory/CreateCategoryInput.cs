namespace FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory
{
    public class CreateCategoryInput
    {
        // Esta classe é um DTO
        public CreateCategoryInput(string name, string? description = null, bool isActive = true)
        {
            Name = name;
            Description = description ?? string.Empty;
            IsActive = isActive;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
