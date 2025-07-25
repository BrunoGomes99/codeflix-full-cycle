﻿using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Repository;
using FC.Codeflix.Catalog.UnitTests.Common;
using Moq;
using Entity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.Common
{
    public class CategoryUseCasesBaseFixture : BaseFixture
    {
        public string GetValidCategoryName()
        {
            string categoryName = string.Empty;
            while (categoryName.Length < 3)
                categoryName = Faker.Commerce.Categories(1)[0];
            if (categoryName.Length > 255)
                categoryName = categoryName[..255]; // Esse operador [..255] garante o retorno dos primeiros 255 caracteres da string
            return categoryName;
        }
        public string GetValidCategoryDescription()
        {
            string categoryDescription = Faker.Commerce.ProductDescription();
            if (categoryDescription.Length > 10000)
                categoryDescription = categoryDescription[..10000];
            return categoryDescription;
        }
        public bool GetRandomBoolean()
            => new Random().NextDouble() < 0.5;

        public Entity.Category GetExampleCategory()
            => new(
                GetValidCategoryName(),
                GetValidCategoryDescription(),
                GetRandomBoolean()
            );

        public Mock<ICategoryRepository> GetRepositoryMock()
            => new();

        public Mock<IUnityOfWork> GetUnityOfWorkMock()
            => new();
    }
}
