using RC.CA.SharedKernel.Extensions;
using Xunit.Abstractions;

namespace RC.CA.WebApi.Tests.Xunit.ExtensionMethods
{
    [Trait("Category", "Extensions")]
    public class GenericExtensions
    {
        private readonly ITestOutputHelper _outputLogger;
        public GenericExtensions(ITestOutputHelper outputLogger)
        {
            _outputLogger = outputLogger;
        }

        public ITestOutputHelper OutputLogger { get; }

        [Theory]
        [InlineData("And", true)]
        [InlineData("", true)]
        [InlineData("Value 1", true)]
        [InlineData("Value 107", false)]
        [InlineData("99", true)]
        public async Task IsInExt_CheckString_Ok(string compareValue, bool expectedResult)
        {

            bool result = compareValue.IsInExt("And", "Or", "Value 1", "Value 2", "17", "20", "99", "");
            Assert.Equal(result, expectedResult);

        }
        [Theory]
        [InlineData(11, false)]
        [InlineData(20, true)]
        [InlineData(452, false)]
        [InlineData(698, false)]
        [InlineData(17, true)]
        public async Task IsInExt_CheckInt_Ok(int compareValue, bool expectedResult)
        {

            bool result = compareValue.IsInExt(10, 20, 50, 100, 17);
            Assert.Equal(result, expectedResult);

        }
        [Theory]
        [DataGenericExtensionsToByteArrayExt]
        public async Task ToByteArrayExt_CheckConvertingEmpty_ReturnEmptyArray(string value, byte[] compareValue)
        {

            var result = value.ToByteArrayExt();

            Assert.Equal(result, compareValue);
        }
        [Fact]
        public async Task ToByteArrayExt_CheckConvertingNull_ReturnEmptyArray()
        {
            string? test = null;

            var result = test.ToByteArrayExt();

            Assert.Equal(result, new byte[0]);
        }
    }
}
