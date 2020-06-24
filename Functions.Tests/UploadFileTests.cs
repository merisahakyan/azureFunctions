using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyFunctionsApp;
using MyFunctionsApp.ServiceInterfaces;
using MyFunctionsApp.Services;
using System.Threading.Tasks;
using Xunit;

namespace Functions.Tests
{
    public class UploadFileTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void UploadFile_Success()
        {
            //Arrange
            var request = TestFactory.CreateHttpRequest("imageUrl", "https://media.gettyimages.com/photos/girl-near-the-mountains-picture-id485316112?s=612x612");
            var blobMock = new Mock<IBlobService>();
            blobMock.Setup(s => s.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var sqlMock = new Mock<ISqlService>();
            sqlMock.Setup(s => s.GetByUrlCountAsync(It.IsAny<string>())).Returns(Task.FromResult(0));
            sqlMock.Setup(s => s.InsertToDbAsync(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var functions = new FunctionDefinitions(blobMock.Object, sqlMock.Object);

            //Act
            var response = await functions.Run(request, logger);

            //Assert
            Assert.IsType<OkResult>(response);
        }


        [Fact]
        public async void UploadFile_EmptyUrl()
        {
            //Arrange
            var request = TestFactory.CreateHttpRequest("imageUrl", "");
            var blobMock = new Mock<IBlobService>();
            blobMock.Setup(s => s.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var sqlMock = new Mock<ISqlService>();
            sqlMock.Setup(s => s.GetByUrlCountAsync(It.IsAny<string>())).Returns(Task.FromResult(0));
            sqlMock.Setup(s => s.InsertToDbAsync(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var functions = new FunctionDefinitions(blobMock.Object, sqlMock.Object);

            //Act
            var response = await functions.Run(request, logger);

            Assert.IsType<BadRequestResult>(response);
        }

        [Fact]
        public async void UploadFile_AlreadyUploadedImage()
        {
            //Arrange
            var request = TestFactory.CreateHttpRequest("imageUrl", "https://media.gettyimages.com/photos/girl-near-the-mountains-picture-id485316112?s=612x612");
            var blobMock = new Mock<IBlobService>();
            blobMock.Setup(s => s.UploadImageAsync(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var sqlMock = new Mock<ISqlService>();
            sqlMock.Setup(s => s.GetByUrlCountAsync(It.IsAny<string>())).Returns(Task.FromResult(1));
            sqlMock.Setup(s => s.InsertToDbAsync(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            var functions = new FunctionDefinitions(blobMock.Object, sqlMock.Object);

            //Act
            var response = await functions.Run(request, logger);

            Assert.IsType<BadRequestResult>(response);
        }
    }
}
