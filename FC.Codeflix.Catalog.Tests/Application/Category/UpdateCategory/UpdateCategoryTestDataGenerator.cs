﻿namespace FC.Codeflix.Catalog.UnitTests.Application.Category.UpdateCategory
{
    public class UpdateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
        {
            var fixture = new UpdateCategoryTestFixture();
            for (int index = 0; index < times; index++)
            {
                var exampleCategory = fixture.GetExampleCategory();
                var exampleInput = fixture.GetValidInput(exampleCategory.Id);

                yield return new object[]
                {
                    exampleCategory,
                    exampleInput
                };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateCategoryTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int index = 0; index < times; index++)
            {
                switch (index % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[]
                        {
                            fixture.GetInvalidInputShortName(),
                            "Name should be at least 3 characters long"
                        });
                        break;

                    case 1:
                        invalidInputsList.Add(new object[]
                        {
                            fixture.GetInvalidInputTooLongName(),
                            "Name should be less or equal 255 characters long"
                        });
                        break;

                    case 2:
                        invalidInputsList.Add(new object[]
                        {
                            fixture.GetInvalidInputTooLongDescription(),
                            "Description should be less or equal 10000 characters long"
                        });
                        break;

                    default:
                        break;
                }
            }

            return invalidInputsList;
        }
    }
}
