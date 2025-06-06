﻿using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.Application.UseCases.Category.Common
{
    public class CategoryModelOutput
    {
        // Esta classe é um DTO
        public CategoryModelOutput(Guid id, string name, string description, bool isActive, DateTime createdAt)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
            CreatedAt = createdAt;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public static CategoryModelOutput FromCategory(DomainEntity.Category category)
            => new (
                category.Id,
                category.Name,
                category.Description,
                category.IsActive,
                category.CreatedAt
            );
    }
}
