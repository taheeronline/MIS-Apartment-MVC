using ManageApartment.Entities;
using ManageApartment.Repositories.Interface;
using ManageApartment.UI.ViewModels.Flat;
using ManageFlat.UI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApartmentXUnitTestProject
{
    public class FlatControllerTests
    {
        private readonly FlatController _controller;
        private readonly Mock<iFlatRepository> _flatServiceMock;
        private readonly Mock<iApartmentRepository> _apartmentServiceMock;
        private readonly Mock<ILogger<FlatController>> _loggerMock;
        private readonly DefaultHttpContext _httpContext;

        public FlatControllerTests()
        {
            _flatServiceMock = new Mock<iFlatRepository>();
            _apartmentServiceMock = new Mock<iApartmentRepository>();
            _loggerMock = new Mock<ILogger<FlatController>>();
            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new Mock<ISession>().Object;
            _controller = new FlatController(_flatServiceMock.Object, _apartmentServiceMock.Object, _loggerMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext
                }
            };
        }

        [Fact]
        public async Task Index_WhenUserIdIsNotNull_ReturnsViewResultWithListOfFlats()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", 1);
            var apartments = new List<Apartment>
            {
                new Apartment { ID = 1, Title = "Apartment 1" }
            };
                    var flats = new List<Flat>
            {
                new Flat { ID = 1, Title = "Flat 1", Description = "Description", Area = 100, Apartment = apartments.First(), IsVacant = true }
            };

            _apartmentServiceMock.Setup(s => s.GetAllApartmentsAsync()).ReturnsAsync(apartments);
            _flatServiceMock.Setup(s => s.GetAllFlatsAsync()).ReturnsAsync(flats);

            // Act
            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as List<vmFlat>;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<vmFlat>>(model);
            Assert.Single(model);
            Assert.Equal("Flat 1", model.First().Title);
        }


        [Fact]
        public async Task Index_WhenUserIdIsNull_RedirectsToHomeIndex()
        {
            // Arrange
            _httpContext.Session.SetInt32("UserId", -1);

            // Act
            var result = await _controller.Index() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public async Task Create_WhenModelIsValid_RedirectsToIndex()
        {
            // Arrange
            var vm = new vmAddFlat { Title = "New Flat", Description = "Description", Area = 120, ApartmentId = 1 };
            _flatServiceMock.Setup(s => s.AddFlatAsync(It.IsAny<Flat>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(vm) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_WhenModelIsInvalid_ReturnsViewWithModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Title", "Required");
            var vm = new vmAddFlat { Title = "", Description = "Description", Area = 120, ApartmentId = 1 };

            // Act
            var result = await _controller.Create(vm) as ViewResult;
            var model = result?.Model as vmAddFlat;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(vm, model);
        }

        [Fact]
        public async Task Edit_WhenIdDoesNotMatchModelId_ReturnsNotFound()
        {
            // Arrange
            var vm = new vmUpdateFlat { ID = 2 };

            // Act
            var result = await _controller.Edit(1, vm) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_WhenModelIsValid_RedirectsToIndex()
        {
            // Arrange
            var vm = new vmUpdateFlat { ID = 1, Title = "Updated Flat", Description = "Updated Description", Area = 130, IsVacant = false, ApartmentId = 1 };
            _flatServiceMock.Setup(s => s.UpdateFlatAsync(It.IsAny<Flat>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(1, vm) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnsRedirectToIndex()
        {
            // Arrange
            _flatServiceMock.Setup(s => s.DeleteFlatAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
