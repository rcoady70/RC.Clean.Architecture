using System.Reflection;
using Xunit.Sdk;

namespace RC.CA.WebApi.Tests.Xunit.ExtensionMethods
{
    internal class DataGenericExtensionsToByteArrayExt : DataAttribute
    {

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { "This is a test array", System.Text.UTF8Encoding.UTF8.GetBytes("This is a test array") };
            yield return new object[] { "'#!\"+2-,./?>", System.Text.UTF8Encoding.UTF8.GetBytes("'#!\"+2-,./?>") };
            yield return new object[] { "", System.Text.UTF8Encoding.UTF8.GetBytes("") };
        }
    }
}
