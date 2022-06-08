using Currency_Converter.Services.Api.Controllers;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Currency_Converter.Domain.Core.Notifications;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Currency_Converter.Domain.Interfaces;
using static Currency_Converter.Services.Api.Startup;
using Currency_Converter.Domain.Users.Repository;
using Currency_Converter.Domain.Conversions.Repository;
using Microsoft.Extensions.Options;
using Currency_Converter.Services.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Currency_Converter.Domain.Users.Commands;

namespace Currency_Converter.Tests.API.UnitTests
{
    public class ConverterControllerTests
    {
        #region Mocks

        public ConverterController converterController;
        public Mock<DomainNotificationHandler> mockNotification;
        public Mock<IMapper> mockMapper;
        public Mock<IMediatorHandler> mockMediator;
        public Mock<ILoggerFactory> mockLoggerFactory;
        public Mock<IOptions<ApiKeys>> mockApiKeys;
        public Mock<BaseController> mockBaseController;
        public Api_Key apiKeyData;

        public ConverterControllerTests()
        {
            mockNotification = new Mock<DomainNotificationHandler>();
            mockMapper = new Mock<IMapper>();
            mockMediator = new Mock<IMediatorHandler>();
            mockLoggerFactory = new Mock<ILoggerFactory>();
            mockApiKeys = new Mock<IOptions<ApiKeys>>();
            mockBaseController = new Mock<BaseController>();

            apiKeyData = new Api_Key()
            {
                Id = "exchangeratesapi",
                ApiKey = "GMFX2f3Z9ynQ5FVbdeZjr2Qr0hBI0E6Z",
                BaseAddress = "https://api.apilayer.com/exchangerates_data"
            };

            var mockUserRepository = new Mock<IUserRepository>();
            var mockConversionRepository = new Mock<IConversionRepository>();

            converterController = new ConverterController(
                mockNotification.Object,
                mockMapper.Object,
                mockLoggerFactory.Object,
                mockMediator.Object,
                mockApiKeys.Object,
                mockUserRepository.Object,
                mockConversionRepository.Object);
        }

        #endregion

        #region User tests

        #region ConverterController_AddUser_Return_Created

        [Fact]
        public void ConverterController_AddUser_Return_Created()
        {
            // Arrange                                                          
            var addUserViewModel = new AddUserViewModel();
            var addUserCommand = 
                new AddUserCommand(
                    string.Format("teste_{0}@gmail.com",DateTime.Now.ToString("yyyyMMdd_HHmmss")));

            mockMapper.Setup(m => m.Map<AddUserCommand>(addUserViewModel)).Returns(addUserCommand);
            mockNotification.Setup(m => m.GetNotifications()).Returns(new List<DomainNotification>());

            // Act
            var result = converterController.PostUser(addUserViewModel);

            // Assert
            mockMediator.Verify(m => m.SendCommand(addUserCommand), Times.Once);
            Assert.IsType<CreatedResult>(result);
        }

        #endregion

        #region ConverterController_AddUser_Return_ModelStateErrors

        [Fact]
        public void ConverterController_AddUser_Return_ModelStateErrors()
        {
            // Arrange                                                          
            var addUserViewModel = new AddUserViewModel();

            var notificationList = new List<DomainNotification> {new DomainNotification("Erro", "ModelError") };
            
            mockNotification.Setup(m => m.GetNotifications()).Returns(notificationList);
            mockNotification.Setup(m => m.HasNotifications()).Returns(true);

            converterController.ModelState.AddModelError("Error", "ModelError");

            // Act
            var result = converterController.PostUser(new AddUserViewModel());

            // Assert
            mockMediator.Verify(m => m.SendCommand(It.IsAny<AddUserCommand>()), Times.Never);
            Assert.IsType<BadRequestObjectResult>(result);

        }

        #endregion

        #region ConverterController_AddUser_Return_DomainErrors

        [Fact]
        public void ConverterController_AddUser_Return_DomainErrors()
        {
            // Arrange                                                          
            var addUserViewModel = new AddUserViewModel();

            var addUserCommand =
                new AddUserCommand(
                    string.Format("teste_{0}@gmail.com", DateTime.Now.ToString("yyyyMMdd_HHmmss")));

            mockMapper.Setup(m => m.Map<AddUserCommand>(addUserViewModel)).Returns(addUserCommand);

            var notificationList = new List<DomainNotification> { new DomainNotification("Error", "Error adding user") };

            mockNotification.Setup(m => m.GetNotifications()).Returns(notificationList);
            mockNotification.Setup(m => m.HasNotifications()).Returns(true);

            // Act
            var result = converterController.PostUser(addUserViewModel);

            // Assert
            mockMediator.Verify(m => m.SendCommand(addUserCommand), Times.Once);
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #endregion        
    }
}
