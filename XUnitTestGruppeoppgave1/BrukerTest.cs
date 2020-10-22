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
    public class BrukerTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IBrukerRepository> mockRep = new Mock<IBrukerRepository>();
        private readonly Mock<ILogger<BrukerController>> mockLog = new Mock<ILogger<BrukerController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task LoggInnOK()
        {
            mockRep.Setup(b => b.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await BrukerController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LoggInnInputIkkeOK()
        {
            mockRep.Setup(b => b.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            BrukerController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await BrukerController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server for innlogging", resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {
            mockRep.Setup(b => b.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await BrukerController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.False((bool)resultat.Value);
        }

        [Fact]
        public void LoggUt()
        {
            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            BrukerController.LoggUt();

            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }

        [Fact]
        public void SjekkInnloggetOK()
        {
            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = BrukerController.SjekkInnlogget() as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Innlogget", resultat.Value);
        }

        [Fact]
        public void SjekkInnloggetIkkeOK()
        {
            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = BrukerController.SjekkInnlogget() as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task HentAlleBrukereInnloggetOK()
        {
            //Arrange
            List<Brukere> brukerListe = new List<Brukere> { It.IsAny<Brukere>() };

            mockRep.Setup(b => b.HentAlleBrukere()).ReturnsAsync(brukerListe);

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            var resultat = await BrukerController.HentAlleBrukere() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Brukere>>((List<Brukere>)resultat.Value, brukerListe);
        }


        [Fact]
        public async Task HentAlleBrukereInnloggetIkkeOK()
        {

            mockRep.Setup(b => b.HentAlleBrukere()).ReturnsAsync(It.IsAny<List<Brukere>>());

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await BrukerController.HentAlleBrukere() as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Ingen brukere funnet", resultat.Value);
        }

        [Fact]
        public async Task HentAlleBrukereIkkeInnlogget()
        {

            mockRep.Setup(b => b.HentAlleBrukere()).ReturnsAsync(It.IsAny<List<Brukere>>());

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(b => b.Session).Returns(mockSession);
            mockSession[_loggetInn] = _ikkeLoggetInn;
            BrukerController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await BrukerController.HentAlleBrukere() as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }


        [Fact]
        public async Task LagGjestebrukerOK()
        {
            Brukere gjesteBruker = new Brukere { Id = 1, Brukernavn = "Gjest", Passord = It.IsAny<byte[]>(), Salt = It.IsAny<byte[]>() };

            mockRep.Setup(b => b.LagGjesteBruker()).ReturnsAsync(gjesteBruker);

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await BrukerController.LagGjesteBruker() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Brukere>((Brukere)resultat.Value, gjesteBruker);
        }

        [Fact]
        public async Task LagGjestebrukerIkkeOK()
        {
            mockRep.Setup(b => b.LagGjesteBruker()).ReturnsAsync(It.IsAny<Brukere>());

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await BrukerController.LagGjesteBruker() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Oppretting av gjestebruker feilet", resultat.Value);
        }

    }
}
