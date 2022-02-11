using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.FirstPracticalWork
{
    /// <summary>
    /// Task 1-1
    /// </summary>
    public static class TaskOne
    {
        class Treangle
        {
            /// <summary>
            /// A special way of storing the sides of a triangle
            /// </summary>
            private IEnumerable<KeyValuePair<char, double>> _sides;

            public Treangle()
            {
            }

            public Treangle(double a, double b, double c)
            {
                SetSides(a, b, c);
            }

            public double A => GetSide('a');
            public double B => GetSide('b');
            public double C => GetSide('c');

            private double GetSide(char name) => _sides.Single(s => s.Key == name).Value;

            public void SetSides(double a, double b, double c)
            {
                if (CheckSidesForZero(a, b, c))
                {
                    throw new FormatException();
                }

                if (!IsExist(a, b, c))
                {
                    throw new ArgumentException();
                }

                _sides = new Dictionary<char, double>
                {
                    { 'a', a },
                    { 'b', b },
                    { 'c', c },
                };
            }

            private static bool IsExist(double a, double b, double c)
            {
                if (a + b > c && a + c > b && b + c > a)
                {
                    return true;
                }

                return false;
            }

            private static bool CheckSidesForZero(params double[] sides)
            {
                return sides.Any(s => s <= 0);
            }

            public double Area()
            {
                var p = _sides.Sum(s => s.Value) / 2;

                return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
            }

            /// <summary>
            /// According to the Pythagorean formula , the definition of a right triangle
            /// </summary>
            /// <returns></returns>
            public bool IsRight()
            {
                var max = _sides.OrderByDescending(s => s.Value).First();

                var sidesSum = _sides.Where(s => s.Key != max.Key)
                    .Sum(s => Math.Pow(s.Value, 2));

                return Math.Pow(max.Value, 2) == sidesSum;
            }
        }

        public class Tests
        {
            [Fact]
            public void Treangle_Area()
            {
                var treangle = new Treangle();

                treangle.SetSides(5, 6, 3);

                var area = treangle.Area();

                Assert.Equal(7.483314773547883, area);
            }

            [Fact]
            public void Treangle_IsNotRight()
            {
                var treangle = new Treangle();

                treangle.SetSides(5, 6, 3);

                bool isRight = treangle.IsRight();

                Assert.False(isRight);
            }

            [Fact]
            public void Treangle_IsRight()
            {
                var treangle = new Treangle();

                treangle.SetSides(5, 6, 7.81);

                bool isRight = treangle.IsRight();

                Assert.False(isRight);
            }
        }
    }
}
