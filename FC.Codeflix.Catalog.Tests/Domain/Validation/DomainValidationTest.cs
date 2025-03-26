using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using System;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            // Arrange
            var value = Faker.Commerce.ProductName();

            //Act
            Action action = () => DomainValidation.NotNull(value, "Value");

            // Assert
            var exceptions = Record.Exception(action);
            Assert.Null(exceptions);
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            // Arrange
            string? value = null;
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            //Act
            Action action = () => DomainValidation.NotNull(value, fieldName);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal($"{fieldName} should not be null", exception.Message);
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            // Arrange
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            // Act
            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal($"{fieldName} should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]       
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            // Arrange
            var target = Faker.Commerce.ProductName();
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            // Act
            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            // Assert
            var exceptions = Record.Exception(action);
            Assert.Null(exceptions);
        }

        [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
        [Trait("Domain", "DomainValidation - Validation")]
        public void MinLengthThrowWhenLess(string target, int minLength)
        {
            // Arrange
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            // Act
            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal($"{fieldName} should be at least {minLength} characters long", exception.Message);
        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
        [Trait("Domain", "DomainValidation - Validation")]
        public void MinLengthOk(string target, int minLength)
        {
            // Arrange
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            // Act            
            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);

            // Assert
            var exceptions = Record.Exception(action);
            Assert.Null(exceptions);
        }

        [Theory(DisplayName = nameof(MaxLengthThrowWhenGreater))]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
        [Trait("Domain", "DomainValidation - Validation")]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            // Arrange
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            // Act
            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            // Assert
            var exception = Assert.Throws<EntityValidationException>(action);
            Assert.Equal($"{fieldName} should be less or equal {maxLength} characters long", exception.Message);
        }

        [Theory(DisplayName = nameof(MaxLengthOk))]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
        [Trait("Domain", "DomainValidation - Validation")]
        public void MaxLengthOk(string target, int maxLength)
        {
            // Arrange
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            // Act
            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            // Assert
            var exceptions = Record.Exception(action);
            Assert.Null(exceptions);
        }

        #region Auxiliar Methods

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests = 5)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfTests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random().Next(1, 20));
                yield return new object[] { example, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests = 5)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfTests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - (new Random().Next(1, 5));
                yield return new object[] { example, minLength };
            }
        }
        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests = 5)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfTests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length - (new Random().Next(1, 5));
                yield return new object[] { example, maxLength };
            }
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests = 5)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfTests - 1; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random().Next(1, 5));
                yield return new object[] { example, minLength };
            }
        }

        #endregion
    }
}
