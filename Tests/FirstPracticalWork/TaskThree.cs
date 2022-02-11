using System.Linq;
using Xunit;

namespace Tests.FirstPracticalWork
{
    /// <summary>
    /// Task 3-3
    /// </summary>
    public static class TaskThree
    {
        internal class Sorter
        {
            public static double[] SortAndFilter(double[] array)
            {
                return array
                    .Select(x => x < 0 ? x * -1 : x)
                    .Distinct()
                    .OrderByDescending(x => x)
                    .ToArray();
            }
        }

        public class Tasts
        {
            [Fact]
            public void Sorter_SortAndFilter()
            {
                var array = new double[] { 1, -2, 3, 1, -3, 12, 9, -3, 6, -8, 10 };

                var sortedArray = Sorter.SortAndFilter(array);

                Assert.True(Enumerable.SequenceEqual(
                        sortedArray,
                        new double[] { 12, 10, 9, 8, 6, 3, 2, 1 }));

                Assert.Equal(1, array[0]);
                Assert.Equal(new double[] { 6, -8 }, array[^3..^1]);
                Assert.Equal(10, array[^1]);
            }
        }
    }
}
