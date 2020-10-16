using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Gruppeoppgave1.Controllers;
using Gruppeoppgave1.DAL;
using Gruppeoppgave1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace XUnitTestGruppeoppgave1
{
    public class BrukerTest
    {
        private readonly Mock<IBrukerRepository> mockRep = new Mock<IBrukerRepository>();
        private readonly Mock<ILogger<BrukerController>> mockLog = new Mock<ILogger<BrukerController>>();

        [Fact]
        public async Task LoggInnTrue()
        {
            mockRep.Setup(s => s.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            var resultat = await BrukerController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(true, resultat.Value);
        }

        [Fact]
        public async Task LoggInnFalse()
        {
            mockRep.Setup(s => s.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var BrukerController = new BrukerController(mockRep.Object, mockLog.Object);

            var resultat = await BrukerController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Innlogging feilet", resultat.Value);
        }
    }
}
