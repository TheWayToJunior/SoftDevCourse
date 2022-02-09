using System;
using System.Linq;
using Xunit;

namespace Refactoring.UnitTests.Laba1
{
    public static class TestTwo
    {
        private static class StringFormatter
        {
            public static string ShortFileString(string path)
            {
                if(path == null)
                {
                    throw new NullReferenceException();
                }

                if(path == string.Empty)
                {
                    return string.Empty;
                }

                var fileName = path
                    .Split('/')
                    .Last()
                    .Split('.')
                    .First()
                    .Select(c => char.IsLower(c) ? char.ToUpper(c) : c);

                return string.Concat(string.Join(null, fileName), ".TMP");
            }
        }

        public class Test
        {
            [Fact]
            public void StringFormatter_ShortFileString()
            {
                var path = "D:/C#/Sandbox/TestProject/TestFile.cs";

                var result = StringFormatter.ShortFileString(path);

                Assert.Equal("TESTFILE.TMP", result);
            }
        }
    }
}
