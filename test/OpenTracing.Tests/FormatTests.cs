using OpenTracing.Propagation;
using Xunit;

namespace OpenTracing.Tests
{
    public class FormatTests
    {
        [Fact]
        public void DummyTestForBuild()
        {
        }

        // TODO: Maintained mostly because I'm not familiar with xunit's conventions
        //[Fact]
        //public void Formats_with_different_names_but_same_type_are_different()
        //{
        //    var format1 = new Format<ITextMap>("format1");
        //    var format2 = new Format<ITextMap>("format2");

        //    Assert.NotEqual(format1, format2);
        //    //Assert.False(format1 == format2);
        //    Assert.NotEqual(format1.GetHashCode(), format2.GetHashCode());
        //    Assert.False(format1.Equals(format2));
        //    Assert.False(Equals(format1, format2));
        //}

        //[Fact]
        //public void Formats_with_different_names_and_different_types_are_different()
        //{
        //    var format1 = new Format<ITextMap>("format1");
        //    var format2 = new Format<string>("format2");

        //    //Assert.False(format1 == format2);
        //    Assert.NotEqual(format1.GetHashCode(), format2.GetHashCode());
        //    Assert.False(format1.Equals(format2));
        //    Assert.False(Equals(format1, format2));
        //}

        //[Fact]
        //public void Formats_with_different_types_but_same_name_are_different()
        //{
        //    var format1 = new Format<ITextMap>("format");
        //    var format2 = new Format<string>("format");

        //    //Assert.False(format1 == format2);
        //    Assert.NotEqual(format1.GetHashCode(), format2.GetHashCode());
        //    Assert.False(format1.Equals(format2));
        //    Assert.False(Equals(format1, format2));
        //}

        //[Fact]
        //public void Formats_with_same_name_and_same_type_are_equal()
        //{
        //    var format1 = new Format<ITextMap>("format");
        //    var format2 = new Format<ITextMap>("format");

        //    Assert.Equal(format1, format2);
        //    //Assert.True(format1 == format2);
        //    Assert.Equal(format1.GetHashCode(), format2.GetHashCode());
        //    Assert.True(format1.Equals(format2));
        //    Assert.True(Equals(format1, format2));
        //}
    }
}
