namespace OpenTracing.Tests
{
    using OpenTracing.Propagation;
    using Xunit;

    public class FormatTests
    {
        [Fact]
        public void Formats_with_different_names_but_same_type_are_different()
        {
            var format1 = new Format<ITextMap>("format1");
            var format2 = new Format<ITextMap>("format2");

            Assert.NotEqual(format1, format2);
        }

        [Fact]
        public void Formats_with_different_names_and_different_types_are_different()
        {
            var format1 = new Format<ITextMap>("format1");
            var format2 = new Format<string>("format2");

            Assert.False(format1.Equals(format2));
        }

        [Fact]
        public void Formats_with_different_types_but_same_name_are_different()
        {
            var format1 = new Format<ITextMap>("format");
            var format2 = new Format<string>("format");

            Assert.False(format1.Equals(format2));
        }

        [Fact]
        public void Formats_with_same_name_and_same_type_are_equal()
        {
            var format1 = new Format<ITextMap>("format");
            var format2 = new Format<ITextMap>("format");

            Assert.Equal(format1, format2);
        }
    }
}