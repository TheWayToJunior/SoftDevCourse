using Microsoft.Extensions.Logging;
using Moq;
using System;

namespace Tests
{
    public static class LoggerTestExtensions
    {
        public static Mock<ILogger<T>> LoggerMock<T>() where T : class
        {
            return new Mock<ILogger<T>>();
        }

        public static Mock<ILogger<T>> VerifyWasCalled<T>(this Mock<ILogger<T>> logger, LogLevel logLevel, string expectedMessage)
        {
            Func<object, Type, bool> state = (v, t) => v.ToString().CompareTo(expectedMessage) == 0;

            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v, t)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            return logger;
        }

        public static Mock<ILogger<T>> VerifyWasCalled<T>(this Mock<ILogger<T>> logger, LogLevel logLevel)
        {
            logger.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            return logger;
        }
    }
}
