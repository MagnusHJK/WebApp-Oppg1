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
        public async Task LagBestillingOK()
        {
            mockRep.Setup(s => s.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await BestillingController.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Bestilling opprettet", resultat.Value);
        }

        [Fact]
        public async Task LagBestillingIkkeOK()
        {
            mockRep.Setup(s => s.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await BestillingController.LagBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Bestilling ble ikke opprettet", resultat.Value);
        }

        [Fact]
        public async Task EndreBestillingInnloggetOK()
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
            Assert.Equal("Bestilling endret", resultat.Value);
        }

        [Fact]
        public async Task EndreBestillingInnloggetIkkeOK()
        {
            mockRep.Setup(s => s.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await BestillingController.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Bestilling ble ikke endret", resultat.Value);
        }

        [Fact]
        public async Task EndreBestillingIkkeInnlogget()
        {
            mockRep.Setup(s => s.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await BestillingController.EndreBestilling(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task SlettBestillingInnloggetOK()
        {
            mockRep.Setup(s => s.SlettBestilling(It.IsAny<int>())).ReturnsAsync(true);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await BestillingController.SlettBestilling(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Bestillig slettet", resultat.Value);
        }


        [Fact]
        public async Task SlettBestillingInnloggetIkkeOK()
        {
            mockRep.Setup(s => s.SlettBestilling(It.IsAny<int>())).ReturnsAsync(false);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            BestillingController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await BestillingController.SlettBestilling(It.IsAny<int>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Bestilling ble ikke slettet", resultat.Value);
        }

        [Fact]
        public async Task SlettBestillingIkkeInnlogget()
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

        [Fact]
        public async Task HentBestillingerOK()
        {
            Bestillinger bestilling = new Bestillinger { Id = 1, Antall = 5, Avgang = It.IsAny<Avganger>(), Bruker = It.IsAny<Brukere>()};
            List<Bestillinger> bestillingListe = new List<Bestillinger> { bestilling };

            mockRep.Setup(s => s.HentBestillinger(It.IsAny<int>())).ReturnsAsync(bestillingListe);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            var resultat = await BestillingController.HentBestillinger(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Bestillinger>>((List<Bestillinger>)resultat.Value, bestillingListe);
        }


        [Fact]
        public async Task HentBestillingerIkkeOK()
        {

            mockRep.Setup(s => s.HentBestillinger(It.IsAny<int>())).ReturnsAsync(It.IsAny<List<Bestillinger>>);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            var resultat = await BestillingController.HentBestillinger(It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Ingen bestillinger funnet", resultat.Value);
        }

        [Fact]
        public async Task HentAlleBestillingerOK()
        {
            Bestillinger bestilling = new Bestillinger { Id = 1, Antall = 5, Avgang = It.IsAny<Avganger>(), Bruker = It.IsAny<Brukere>() };
            List<Bestillinger> bestillingListe = new List<Bestillinger> { bestilling };

            mockRep.Setup(s => s.HentAlleBestillinger()).ReturnsAsync(bestillingListe);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            var resultat = await BestillingController.HentAlleBestillinger() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Bestillinger>>((List<Bestillinger>)resultat.Value, bestillingListe);
        }


        [Fact]
        public async Task HentAlleBestillingerIkkeOK()
        {

            mockRep.Setup(s => s.HentBestillinger(It.IsAny<int>())).ReturnsAsync(It.IsAny<List<Bestillinger>>);
            var BestillingController = new BestillingController(mockRep.Object, mockLog.Object);

            var resultat = await BestillingController.HentBestillinger(It.IsAny<int>()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Ingen bestillinger funnet", resultat.Value);
        }

    }
}
