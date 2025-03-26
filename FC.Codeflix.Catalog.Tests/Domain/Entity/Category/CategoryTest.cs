using Xunit;
using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest
    {
        private readonly CategoryTestFixture _categoryTestFixture = new CategoryTestFixture();

        public CategoryTest(CategoryTestFixture categoryTestFixture)
        {
            _categoryTestFixture = categoryTestFixture;
        }

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var dateTimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
            var dateTimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validCategory.Name, category.Name);
            Assert.Equal(validCategory.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt >= dateTimeBefore);
            Assert.True(category.CreatedAt <= dateTimeAfter);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var dateTimeBefore = DateTime.Now;

            // Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
            var dateTimeAfter = DateTime.Now;

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validCategory.Name, category.Name);
            Assert.Equal(validCategory.Description, category.Description);
            Assert.NotEqual(default(Guid), category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.CreatedAt >= dateTimeBefore);
            Assert.True(category.CreatedAt <= dateTimeAfter);
            Assert.Equal(isActive, category.IsActive);

            // Usando FluentAssertions para fazer os Asserts para fins de exemplo
            category.Should().NotBeNull();
            category.Name.Should().Be(validCategory.Name);
            category.Description.Should().Be(validCategory.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
            (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
            category.IsActive.Should().Be(isActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => new DomainEntity.Category(name!, validCategory.Description);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null", exception.Message);

            // Usando FluentAssertions para fazer os Asserts para fins de exemplo
            action.Should().Throw<EntityValidationException>().WithMessage("Name should not be empty or null");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsNull()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => new DomainEntity.Category(validCategory.Name, null!);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should not be null", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(x => "a").ToArray());

            // Act
            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange            
            var validCategory = _categoryTestFixture.GetValidCategory();
            var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(x => "a").ToArray());
            
            // Act
            Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            // Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
            category.Activate();

            // Assert
            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            // Act
            var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
            category.Deactivate();

            // Assert
            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();
            var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

            // Act
            category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

            // Assert
            Assert.Equal(categoryWithNewValues.Name, category.Name);
            Assert.Equal(categoryWithNewValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();
            var currentDescription = category.Description;
            var newName = _categoryTestFixture.GetValidCategoryName();

            // Act
            category.Update(newName);

            // Assert
            Assert.Equal(newName, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }

        [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void UpdateErrorWhenNameIsEmpty(string? name)
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => category.Update(name!);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Theory(DisplayName = nameof(UpadateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
        public void UpadateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();

            // Act
            Action action = () => category.Update(invalidName);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();
            //var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(x => "a").ToArray());
            var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

            // Act
            Action action = () => category.Update(invalidName);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();
            ///var invalidDescription = String.Join(null, Enumerable.Range(0, 10001).Select(x => "a").ToArray());
            var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
            while (invalidDescription.Length <= 10000)
                invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}"; 

            // Act
            Action action = () => category.Update("Category New Name", invalidDescription);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal("Description should be less or equal 10000 characters long", exception.Message);
        }

        #region Auxiliar Methods

        // O método abaixo, apesar de retornar uma string por vez, deve ser declarado para retornar um
        // array/IEnumerable, uma vez que o MemberData espera uma coleção para qual ele possa iterar sobre.
        // Além disso, foi array de object para ficar mais genérico, mas funcionaria se eu colocasse array de string tbm
        public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
        {
            var fixture = new CategoryTestFixture();

            // O yield return garante que a cada iteração do for,
            // seja feito um return de um valor, sem acabar a execução do método
            for (int i = 0; i < numberOfTests; i++)
            {
                bool isOdd = i % 2 == 1;
                yield return new object[] {
                    fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
                };
            }
        }

        #endregion Auxiliar Methods
    }
}
