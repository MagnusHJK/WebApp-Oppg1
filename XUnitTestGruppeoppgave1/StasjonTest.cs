using Gruppeoppgave1.Models;
using Gruppeoppgave1.DAL;
using Gruppeoppgave1.Controllers;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace XUnitTestGruppeoppgave1
{
    public class StasjonTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IStasjonRepository> mockRep = new Mock<IStasjonRepository>();
        private readonly Mock<ILogger<StasjonController>> mockLog = new Mock<ILogger<StasjonController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();


        [Fact]
        public async Task HentAlleTrue()
        {
            mockRep.Setup(s => s.HentAlleStasjoner()).ReturnsAsync(It.IsAny<List<Stasjoner>>());

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await stasjonController.HentAlleStasjoner() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task HentAlleFalse()
        {
            mockRep.Setup(s => s.HentAlleStasjoner()).ReturnsAsync(It.IsAny<List<Stasjoner>>());

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            //Act
            var resultat = await stasjonController.HentAlleStasjoner() as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ingen stasjoner", resultat.Value);
        }



        [Fact]
        public async Task HentEnTrue()
        {
            // Arrange
            var stasjon1 = new Stasjoner
            {
                Id = 1,
                Navn = "Fredrikstad"
            };

            mockRep.Setup(s => s.HentEnStasjon(It.IsAny<int>())).ReturnsAsync(stasjon1);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            // Act
            var resultat = await stasjonController.HentEnStasjon(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Stasjoner>(stasjon1, (Stasjoner)resultat.Value);
        }

        [Fact]
        public async Task HentEnFalse()
        {
            mockRep.Setup(s => s.HentEnStasjon(It.IsAny<int>())).ReturnsAsync(It.IsAny<Stasjoner>);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            var resultat = await stasjonController.HentEnStasjon(It.IsAny<int>()) as NotFoundObjectResult;
            
            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ikke stasjonen", resultat.Value);
        }


        [Fact]
        public async Task LagStasjonTrueInnlogget()
        {

            mockRep.Setup(s => s.LagStasjon(It.IsAny<Stasjon>())).ReturnsAsync(true);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            stasjonController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await stasjonController.LagStasjon(It.IsAny<Stasjon>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LagStasjonIkkeInnlogget()
        {
            mockRep.Setup(s => s.LagStasjon(It.IsAny<Stasjon>())).ReturnsAsync(true);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            stasjonController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await stasjonController.LagStasjon(It.IsAny<Stasjon>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task EndreStasjonInnlogget()
        {
            mockRep.Setup(s => s.EndreStasjon(It.IsAny<Stasjon>())).ReturnsAsync(true);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            stasjonController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await stasjonController.EndreStasjon(It.IsAny<Stasjon>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);

        }

        [Fact]
        public async Task EndreStasjonIkkeInnlogget()
        {
            mockRep.Setup(s => s.EndreStasjon(It.IsAny<Stasjon>())).ReturnsAsync(true);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            stasjonController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await stasjonController.EndreStasjon(It.IsAny<Stasjon>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);

        }

        [Fact]
        public async Task SlettStasjonInnlogget()
        {
            mockRep.Setup(s => s.SlettStasjon(It.IsAny<int>())).ReturnsAsync(true);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            stasjonController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await stasjonController.SlettStasjon(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);

        }


        [Fact]
        public async Task SlettStasjonIkkeInnlogget()
        {
            mockRep.Setup(s => s.SlettStasjon(It.IsAny<int>())).ReturnsAsync(true);

            var stasjonController = new StasjonController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            stasjonController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await stasjonController.SlettStasjon(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }
    }
}
