using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryTestFixture))]
    public class UpdateCategoryInputValidatorTest
    {
        private readonly UpdateCategoryTestFixture _fixture;

        public UpdateCategoryInputValidatorTest(UpdateCategoryTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
        [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
        public void DontValidateWhenEmptyGuid()
        {
            // Arrange
            var input = _fixture.GetValidInput(Guid.Empty);
            var validator = new UpdateCategoryInputValidator();

            // Act
            var validationResult = validator.Validate(input);

            // Assert
            Assert.NotNull(validationResult);
            Assert.False(validationResult.IsValid);
            Assert.Single(validationResult.Errors);
            Assert.True(validationResult.Errors[0].ErrorMessage.Equals("'Id' must not be empty.") ||
                        validationResult.Errors[0].ErrorMessage.Equals("'Id' deve ser informado."));
        }


        [Fact(DisplayName = nameof(ValidateWhenValid))]
        [Trait("Application", "UpdateCategoryInputValidator - Use Cases")]
        public void ValidateWhenValid()
        {
            // Arrange
            var input = _fixture.GetValidInput();
            var validator = new UpdateCategoryInputValidator();

            // Act
            var validationResult = validator.Validate(input);

            // Assert
            Assert.NotNull(validationResult);
            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Errors);
        }
    }
}
