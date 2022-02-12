using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.SecondPracticalWork
{
    /// <summary>
    /// Task 2-1
    /// </summary>
    public class TaskTwo
    {
        public class FileService
        {
            private readonly IFileExplorer _fileExplorer;
            private readonly ILogger<FileService> _logger;

            public FileService(IFileExplorer fileExplorer, ILogger<FileService> logger)
            {
                _fileExplorer = fileExplorer;
                _logger = logger;
            }

            public async Task<int> RemoveTempraryFilesAsync(string dir)
            {
                int filesSize = 0;
                var files = await _fileExplorer.ReadAsync(dir);

                foreach (var fileName in files)
                {
                    bool isExists = await _fileExplorer.ExistsAsync(fileName);

                    if (!isExists)
                    {
                        _logger.LogWarning($"File not found {fileName}");
                        continue;
                    }

                    filesSize += await _fileExplorer.GetFileSizeAsync(fileName);
                    var isRemoved = await _fileExplorer.RemoveAsync(fileName);

                    if (!isRemoved)
                    {
                        _logger.LogWarning($"File not deleted {fileName}");
                        continue;
                    }
                }

                return filesSize;
            }
        }

        public class Tests
        {
            [Fact]
            public async Task FileService_RemoveTempraryFilesAsync_TotalFileSize()
            {
                var fileNames = new[] { "С:/TestFolder/file1.txt", "С:/TestFolder/file2.txt" };

                Mock<IFileExplorer> explorerMock = new();
                explorerMock.Setup(ex => ex.ReadAsync(It.IsAny<string>()))
                    .ReturnsAsync(fileNames);

                explorerMock.Setup(ex => ex.ExistsAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);
                explorerMock.Setup(ex => ex.RemoveAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);

                explorerMock.Setup(ex => ex.GetFileSizeAsync(fileNames[0]))
                    .ReturnsAsync(45);
                explorerMock.Setup(ex => ex.GetFileSizeAsync(fileNames[1]))
                    .ReturnsAsync(55);

                var loggerMock = LoggerTestExtensions.LoggerMock<FileService>();

                var service = new FileService(explorerMock.Object, loggerMock.Object);

                int filesSize = await service.RemoveTempraryFilesAsync("C:/TestFolder");

                Assert.Equal(100, filesSize);
            }

            [Fact]
            public async Task FileService_RemoveTempraryFilesAsync_FileNotFound()
            {
                var fileNames = new[] { "С:/TestFolder/file1.txt"};

                Mock<IFileExplorer> explorerMock = new();
                explorerMock.Setup(ex => ex.ReadAsync(It.IsAny<string>()))
                    .ReturnsAsync(fileNames);
                explorerMock.Setup(ex => ex.ExistsAsync(It.IsAny<string>()))
                    .ReturnsAsync(false);

                var loggerMock = LoggerTestExtensions.LoggerMock<FileService>();

                var service = new FileService(explorerMock.Object, loggerMock.Object);

                int filesSize = await service.RemoveTempraryFilesAsync("C:/TestFolder");

                Assert.Equal(0, filesSize);
                loggerMock.VerifyWasCalled(LogLevel.Warning, "File not found С:/TestFolder/file1.txt");
            }

            [Fact]
            public async Task FileService_RemoveTempraryFilesAsync_FileNotDeleted()
            {
                var fileNames = new[] { "С:/TestFolder/file1.txt" };

                Mock<IFileExplorer> explorerMock = new();
                explorerMock.Setup(ex => ex.ReadAsync(It.IsAny<string>()))
                    .ReturnsAsync(fileNames);
                explorerMock.Setup(ex => ex.ExistsAsync(It.IsAny<string>()))
                    .ReturnsAsync(true);
                explorerMock.Setup(ex => ex.RemoveAsync(It.IsAny<string>()))
                    .ReturnsAsync(false);

                var loggerMock = LoggerTestExtensions.LoggerMock<FileService>();

                var service = new FileService(explorerMock.Object, loggerMock.Object);

                int filesSize = await service.RemoveTempraryFilesAsync("C:/TestFolder");

                Assert.Equal(0, filesSize);
                loggerMock.VerifyWasCalled(LogLevel.Warning, "File not deleted С:/TestFolder/file1.txt");
            }
        }
    }

    public interface IFileExplorer
    {
        Task<bool> ExistsAsync(string fileName);

        Task<int> GetFileSizeAsync(string fileName);

        Task<IEnumerable<string>> ReadAsync(string fileName);

        Task<bool> RemoveAsync(string fileName);
    }
}
