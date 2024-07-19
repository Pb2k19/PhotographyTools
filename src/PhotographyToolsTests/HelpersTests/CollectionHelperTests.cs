using Photography_Tools.Helpers;

namespace PhotographyToolsTests.HelpersTests
{
    public class CollectionHelperTests
    {
        private static readonly Dictionary<string, DateTime> DateTimeTestDictionary = new()
        {
            { "Test1", new(1900, 5, 6, 7, 18, 14) },
            { "Test2", new(2134, 5, 6, 7, 18, 41) },
            { "Test3", new(2500, 5, 6, 7, 18, 7) },
        };

        [Fact]
        public void GetValueOrNull_ShouldReturnValue()
        {
            DateTime expected = new(2134, 5, 6, 7, 18, 41);

            DateTime? actual = DateTimeTestDictionary.GetValueOrNull("Test2");

            Assert.NotNull(actual);
            Assert.Equal(expected, actual.Value);
        }

        [Fact]
        public void GetValueOrNull_ShouldReturnNull()
        {
            DateTime? actual = DateTimeTestDictionary.GetValueOrNull("T");

            Assert.Null(actual);
        }

        [Fact]
        public void GetValueOrNull_ShouldArgumentNullException()
        {
            Dictionary<string, int>? nullDictionary = null;

#pragma warning disable CS8604 // Possible null reference argument.
            Assert.Throws<ArgumentNullException>(() => nullDictionary.GetValueOrNull("Test2"));
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
