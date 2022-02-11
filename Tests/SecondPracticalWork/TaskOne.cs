using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Tests.SecondPracticalWork
{
    /// <summary>
    /// Task 1-3
    /// </summary>
    public static class TaskOne
    {
        private class Signal
        {
            private readonly IFileWriterFactory _fileWriterFactory;

            public Signal(IFileWriterFactory fileWriterFactory)
            {
                _fileWriterFactory = fileWriterFactory;
            }

            public double[] Samples { get; set; }

            public void FullRectify(params double[] array)
            {
                Samples = FirstPracticalWork.TaskThree.Sorter.SortAndFilter(array);

                using var fileWriter = _fileWriterFactory.Create();

                fileWriter.ArrayWriteLine(array.Union(Samples));
                fileWriter.ArrayWriteLine(array.Except(Samples));
                fileWriter.ArrayWriteLine(array.Intersect(Samples));
            }
        }

        public sealed class Tests
        {
            [Fact]
            public void Signal_FullRectify()
            {
                var writerMock = new Mock<IFileWriter>();
                writerMock.Setup(x => x.ArrayWriteLine(It.IsAny<IEnumerable<double>>())).Verifiable();

                var factoryMock = new Mock<IFileWriterFactory>();
                factoryMock.Setup(f => f.Create()).Returns(writerMock.Object);

                Signal signal = new(factoryMock.Object);

                signal.FullRectify(1, -2, 3, 1, -3, 12, 9, -3, 6, -8, 10);

                var sortedArray = signal.Samples;

                Assert.True(Enumerable.SequenceEqual(
                        sortedArray,
                        new double[] { 12, 10, 9, 8, 6, 3, 2, 1 }));
            }
        }
    }

    internal interface IFileWriterFactory
    {
        IFileWriter Create();
    }

    public interface IFileWriter : IDisposable
    {
        void ArrayWriteLine(IEnumerable<double> enumerable);
    }
}
