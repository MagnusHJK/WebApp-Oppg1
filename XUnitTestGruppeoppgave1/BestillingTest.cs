using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gruppeoppgave1.Controllers;
using Gruppeoppgave1.DAL;
using Gruppeoppgave1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace XUnitTestGruppeoppgave1
{
    public class BestillingTest
    {

        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IBestillingRepository> mockRep = new Mock<IBestillingRepository>();
        private readonly Mock<ILogger<BestillingController>> mockLog = new Mock<ILogger<BestillingController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();


        [Fact]
        public async Task LagBestillingTrue()
        {
            mockRep.Setup(s => s.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await BestillingController.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LagBestillingFalse()
        {
            mockRep.Setup(s => s.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await BestillingController.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Bestilling Feilet", resultat.Value);
        }

        [Fact]
        public async Task EndreBestillingTrue()
        {
            mockRep.Setup(s => s.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await BestillingController.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task EndreBestillingFalse()
        {
            mockRep.Setup(s => s.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await BestillingController.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
    public async Task SlettBestillingTrue()
        {
            mockRep.Setup(s => s.SlettBestilling(It.IsAny<int>())).ReturnsAsync(true);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await BestillingController.SlettBestilling(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task SlettBestillingFalse()
        {
            mockRep.Setup(s => s.SlettBestilling(It.IsAny<int>())).ReturnsAsync(false);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await BestillingController.SlettBestilling(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        //TODO Fikse HentAlleBestillingerTest


    }
}
