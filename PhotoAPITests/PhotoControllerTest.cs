using System.IO;
using System.Text;
using PhotoAPI.Services;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using PhotoAPI.Controllers;
using Microsoft.AspNetCore.Http;
using PhotoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PhotoAPITests;

public class PhotoControllerTest
{
    [Fact]
    public async Task AddPhoto()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<PhotoController>>();
        var mockPhotoService = new Mock<IPhotoService>();
        var mockPhotoUploadService = new Mock<IPhotoUploadService>();
        var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        var controller = new PhotoController(mockPhotoService.Object, mockLogger.Object, mockWebHostEnvironment.Object, mockPhotoUploadService.Object);
        var fileMock = new Mock<IFormFile>();
        var content = "Hello World from a Fake File";
        var fileName = "test.txt";
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(content.Length);
        fileMock.Setup(_ => _.OpenReadStream()).Returns(new MemoryStream(Encoding.UTF8.GetBytes(content)));
        mockPhotoUploadService.Setup(x => x.UploadPhoto(It.IsAny<IFormFile>(), It.IsAny<string>())).ReturnsAsync("http://www.example.com");
        mockPhotoService.Setup(x => x.Add(It.IsAny<Photo>())).ReturnsAsync(new Photo());

        // Act
        var result = await controller.AddPhoto("Test", fileMock.Object) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
}
