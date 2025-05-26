using FC.Codeflix.Catalog.Application.UseCases.Category.DeleteCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.DeleteCategory
{
    [Collection(nameof(DeleteCategoryTestFixture))]
    public class DeleteCategoryInputValidatorTest
    {
        private readonly DeleteCategoryTestFixture _fixture;

        public DeleteCategoryInputValidatorTest(DeleteCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(ValidationOk))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public void ValidationOk()
        {
            // Arrange
            var validInput = new DeleteCategoryInput(Guid.NewGuid());
            var validator = new DeleteCategoryInputValidator();

            // Act
            var validationResult = validator.Validate(validInput);

            // Assert
            Assert.NotNull(validationResult);
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Errors);
        }

        [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
        [Trait("Application", "DeleteCategory - Use Cases")]
        public void InvalidWhenEmptyGuidId()
        {
            // Arrange
            var invalidInput = new DeleteCategoryInput(Guid.Empty);
            var validator = new DeleteCategoryInputValidator();

            // Act
            var validationResult = validator.Validate(invalidInput);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.True(validationResult.Errors[0].ErrorMessage.Equals("'Id' must not be empty.") ||
                        validationResult.Errors[0].ErrorMessage.Equals("'Id' deve ser informado."));
        }
    }
}
