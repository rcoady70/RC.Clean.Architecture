using System.Text;
using RC.CA.SharedKernel.Extensions;

namespace RC.CA.WebApi.Tests.Xunit.ExtensionMethods
{
    [Trait("Category", "Extensions")]
    public class StringExtensions
    {

        [Fact]
        public async Task ToByteArrayExt_CheckConverting_Ok()
        {
            var test = "Test string is a long string of utf8";

            var result = test.ToByteArrayExt();

            Assert.Equal(result, Encoding.UTF8.GetBytes(test));
        }
        [Fact]
        public async Task ToByteArrayExt_CheckConvertingEmpty_ReturnEmptyArray()
        {
            var test = "";

            var result = test.ToByteArrayExt();

            Assert.Equal(result, new byte[0]);
        }
        [Fact]
        public async Task ToByteArrayExt_CheckConvertingNull_ReturnEmptyArray()
        {
            string? test = null;

            var result = test.ToByteArrayExt();

            Assert.Equal(result, new byte[0]);
        }

        [Theory]
        [InlineData("tom@gmail.com", true)]
        [InlineData("tom'j@gmailcom", false)]
        [InlineData("\"Tom James\"", false)]
        [InlineData("just test", false)]
        [InlineData("\"Tom James\" <tomjames@gmail.com>", true)]
        [InlineData("<tomjames@gmail.com>", true)]
        [InlineData("<tomjames@gmail.com<", false)]
        [InlineData("", false)]
        public async Task IsValidEmailExt_ValidEmail_False(string email, bool compareValue)
        {

            var result = email.IsValidEmailExt();
            Assert.Equal(result, compareValue);

        }
    }
}
