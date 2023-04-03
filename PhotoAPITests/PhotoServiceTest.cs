using System;
using Xunit;
using Moq;
using PhotoAPI.Repository;
using PhotoAPI.Models;
using PhotoAPI.Services;

namespace PhotoAPITest
{
    public class PhotoServiceTests
    {
        private readonly Mock<IPhotoRespository> _mockRepository;
        private readonly IPhotoService? _service;

        public PhotoServiceTests()
        {
            _mockRepository = new Mock<IPhotoRespository>();
            //_service = new Mock<IPhotoService>();
        }

        [Fact]
        public async Task Add_ValidPhoto_ReturnsNewPhoto()
        {
            // Arrange
            var photo = new Photo { Name = "Test"};
            var expectedPhoto = new Photo { Id = Guid.NewGuid(), Name = "Test" , Url="abc"};
            _mockRepository.Setup(x => x.Add(photo)).ReturnsAsync(expectedPhoto);

            // Act
            var result = await _service.Add(photo);

            // Assert
            Assert.Equal(expectedPhoto, result);
        }
    }
}