using AutoMapper;
using MagicVilla.Api.Models;
using MagicVilla.Api.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagicVilla.Api.Controllers;
using MagicVilla.Api.Models.Dto;

namespace MagicVilla.Web.Api.UnitTests
{
    [TestFixture]
    public class VillaControllerTests
    {
        private VillaController _controller;
        private Mock<ILogger<VillaController>> _loggerMock;
        private Mock<IVillaRepository> _villaRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<VillaController>>();
            _villaRepositoryMock = new Mock<IVillaRepository>();
            _mapperMock = new Mock<IMapper>();

            _controller = new VillaController(_loggerMock.Object, _villaRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetVillas_ReturnsOkResponse()
        {
            // Arrange
            var villas = new List<Villa>();
            _villaRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Villa, bool>>>(), null))
                .ReturnsAsync(villas);

            _mapperMock.Setup(mapper => mapper.Map<List<VillaDto>>(villas)).Returns(new List<VillaDto>());

            // Act
            var result = await _controller.GetVillas();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetVillas_ReturnsApiResponseWithMappedData()
        {
            // Arrange
            var villas = new List<Villa>();
            _villaRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Villa, bool>>>(), null))
                .ReturnsAsync(villas);

            var mappedData = new List<VillaDto>();
            _mapperMock.Setup(mapper => mapper.Map<List<VillaDto>>(villas)).Returns(mappedData);

            // Act
            var result = await _controller.GetVillas();
            var okResult = result.Result as OkObjectResult;
            var apiResponse = okResult.Value as ApiResponse;

            // Assert
            Assert.AreEqual(mappedData, apiResponse.Result);
        }


        [TearDown]
        public void TearDown()
        {
            // Clean up
            _controller = null;
            _loggerMock = null;
            _villaRepositoryMock = null;
            _mapperMock = null;
        }
    }


}
