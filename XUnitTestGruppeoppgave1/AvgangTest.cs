using System;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Gruppeoppgave1.DAL;
using Gruppeoppgave1.Controllers;
using Gruppeoppgave1.Models;

namespace XUnitTestGruppeoppgave1
{
    public class AvgangTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IAvgangRepository> mockRep = new Mock<IAvgangRepository>();
        private readonly Mock<ILogger<AvgangController>> mockLog = new Mock<ILogger<AvgangController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task LagAvgangInnlogget()
        {
            DateTime dato = new DateTime(2020,10,10);

            mockRep.Setup(s => s.LagAvgang(It.IsAny<int>(), It.IsAny<int>(), dato.ToString(), It.IsAny<int>())).ReturnsAsync(true);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            avgangController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await avgangController.LagAvgang(It.IsAny<int>(), It.IsAny<int>(), dato.ToString(), It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LagAvgangIkkeInnlogget()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(s => s.LagAvgang(It.IsAny<int>(), It.IsAny<int>(), dato.ToString(), It.IsAny<int>())).ReturnsAsync(true);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            avgangController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await avgangController.LagAvgang(It.IsAny<int>(), It.IsAny<int>(), dato.ToString(), It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task EndreAvgangInnlogget()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(s => s.EndreAvgang(It.IsAny<int>(), dato.ToString(), It.IsAny<int>())).ReturnsAsync(true);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            avgangController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await avgangController.EndreAvgang(It.IsAny<int>(), dato.ToString(), It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);

        }

        [Fact]
        public async Task EndreAvgangIkkeInnlogget()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(s => s.EndreAvgang(It.IsAny<int>(), dato.ToString(), It.IsAny<int>())).ReturnsAsync(true);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            avgangController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await avgangController.EndreAvgang(It.IsAny<int>(), dato.ToString(), It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);

        }

        [Fact]
        public async Task SlettAvgangInnlogget()
        {
            mockRep.Setup(s => s.SlettAvgang(It.IsAny<int>())).ReturnsAsync(true);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            avgangController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await avgangController.SlettAvgang(It.IsAny<int>()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);

        }

        [Fact]
        public async Task SlettAvgangIkkeInnlogget()
        {
            mockRep.Setup(s => s.SlettAvgang(It.IsAny<int>())).ReturnsAsync(false);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            avgangController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var resultat = await avgangController.SlettAvgang(It.IsAny<int>()) as UnauthorizedObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke innlogget", resultat.Value);
        }

        [Fact]
        public async Task SjekkAvgangerTrue()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(a => a.SjekkAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString())).ReturnsAsync(true);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.SjekkAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task SjekkAvgangerFalse()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(a => a.SjekkAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString())).ReturnsAsync(false);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.SjekkAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Avgangen eksisterer ikke", resultat.Value);
        }

        [Fact]
        public async Task GenererAvgangerTrue()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(a => a.SjekkAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString())).ReturnsAsync(true);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.SjekkAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task GenererAvgangerFalse()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(a => a.GenererAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString())).ReturnsAsync(false);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.GenererAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Avgangen eksisterer ikke", resultat.Value);
        }

        [Fact]
        public async Task HentAvgangerTrue()
        {
            //Arrange
            DateTime dato = new DateTime(2020, 10, 10);
            var stasjon1 = new Stasjoner { Id = 1, Navn = "Fredrikstad" };
            var stasjon2 = new Stasjoner { Id = 2, Navn = "Bærum" };
            var avgang1 = new Avganger { Id = 1, StasjonFra = stasjon1, StasjonTil = stasjon2, Dato = dato, Pris = 200 };
            var avgangerListe = new List<Avganger>();
            avgangerListe.Add(avgang1);

            mockRep.Setup(a => a.HentAvganger(avgang1.StasjonFra.Id, avgang1.StasjonTil.Id, avgang1.Dato.ToString())).ReturnsAsync(avgangerListe);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.HentAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString()) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Avganger>>((List<Avganger>)resultat.Value, avgangerListe);
        }

        [Fact]
        public async Task HentAvgangerFalse()
        {
            //Arrange
            DateTime dato = new DateTime(2020, 10, 10);
            var stasjon1 = new Stasjoner { Id = 1, Navn = "Fredrikstad" };
            var stasjon2 = new Stasjoner { Id = 2, Navn = "Bærum" };
            var avgang1 = new Avganger { Id = 1, StasjonFra = stasjon1, StasjonTil = stasjon2, Dato = dato, Pris = 200 };
            var avgangerListe = new List<Avganger>();
            avgangerListe.Add(avgang1);

            mockRep.Setup(a => a.HentAvganger(avgang1.StasjonFra.Id, avgang1.StasjonTil.Id, avgang1.Dato.ToString())).ReturnsAsync(avgangerListe);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.HentAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Avganger ble ikke hentet", resultat.Value);
        }


        [Fact]
        public async Task HentAlleAvgangerTrue()
        {
            //Arrange
            DateTime dato = new DateTime(2020, 10, 10);
            var stasjon1 = new Stasjoner { Id = 1, Navn = "Fredrikstad" };
            var stasjon2 = new Stasjoner { Id = 2, Navn = "Bærum" };
            var avgang1 = new Avganger { Id = 1, StasjonFra = stasjon1, StasjonTil = stasjon2, Dato = dato, Pris = 200 };
            var avgangerListe = new List<Avganger>();
            avgangerListe.Add(avgang1);

            mockRep.Setup(a => a.HentAlleAvganger()).ReturnsAsync(avgangerListe);

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.HentAlleAvganger() as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Avganger>>((List<Avganger>)resultat.Value, avgangerListe);
        }

        [Fact]
        public async Task HentAlleAvgangerFalse()
        {
            DateTime dato = new DateTime(2020, 10, 10);
            mockRep.Setup(a => a.HentAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString())).ReturnsAsync(It.IsAny<List<Avganger>>());

            var avgangController = new AvgangController(mockRep.Object, mockLog.Object);

            var resultat = await avgangController.HentAvganger(It.IsAny<int>(), It.IsAny<int>(), dato.ToString()) as NotFoundObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ingen avganger", resultat.Value);
        }

    }
}
