using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using profil.api;
using profil.api.Controllers;
using profil.api.Models;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Profil.Api.Test
{

    public class ConsultantProfileControllerTest
    {
        [Fact]
        public void Unauthorized_user_can_get_profiles_containing_only_first_names_from_api()
        {

            //         //eftersom controllern hämtar data från repository.GetAllLimited() vilken i sin tur hämtar från GetAll() behöver vi mocka GetAll().
            //         var repoMock = new Mock<IConsultantProfileRepository>();
            //         //repoMock.Setup(x => x.GetAll()).Returns(GetMockedConsultantProfiles().AsQueryable);
            //         //CallBase anropar den riktiga metoden, måste dock sättas upp här eftersom klassen är mockad. 
            //         //repoMock.Setup(x => x.GetAllLimited()).CallBase();
            //var controller = new ConsultantProfileController(repoMock.Object);

            // Av någon anledning fick jag inte detta, ovan, att fungera och valde därför att använda InMemory-repot direkt, när databasen implementeras ger vi oss på att mocka den istället,
            // nedan är felmddelandet från försöket med CallBase.
            //This is a DynamicProxy2 error: The interceptor attempted to 'Proceed' for method 'System.Collections.Generic.IEnumerable`1[profil.api.Models.ConsultantProfileLimitedViewModel] GetAllLimited()' 
            //which has no target.When calling method without target there is no implementation to 'proceed' to and it is the responsibility 
            //of the interceptor to mimic the implementation(set return value, out arguments etc) at Castle.DynamicProxy.AbstractInvocation.ThrowOnNoTarget()

            var repo = new InMemoryConsultantProfileRepository();
            var controller = new ConsultantProfileController(repo);

            //eftrersom vi vill se hela objektet som json, precis så som klienten får data, behöver vi validera det serialiserade objektet
            var expectedJson = JsonConvert.SerializeObject(new { FirstName = "Jimi" });

            //serialisera första objektet
            var actualJson = JsonConvert.SerializeObject(controller.GetLimitedConsultantProfiles().First());

            actualJson.ShouldBeEquivalentTo(expectedJson, "_ Endast förnamnet får lämnas ut till icke registrerade användare");
        }

        [Fact]
        public void Authorized_user_can_get_full_profile_info_from_api()
        {
            var repoMock = new Mock<IConsultantProfileRepository>();
            repoMock.Setup(x => x.GetAll()).Returns(GetMockedConsultantProfiles);

            //För att kunna hämta data som en autentiserad användare behöver vi manipulera CurrentUser, vilken finns på ControllerContext.HttpContext. 
            //I "vanliga" .Net görs detta med "Thread.CurrentPrincipal = new GenericPrincipal..."
            var currentUser = GetMockedCurrentUser("Jimi");
            var mockedControllerContext = GetMockedHttpContext_WithCurrentUser(currentUser);

            var controller = new ConsultantProfileController(repoMock.Object)
            {
                ControllerContext = mockedControllerContext
            };

            var expectedJson = JsonConvert.SerializeObject(new
            {
                FirstName = "Jimi",
                LastName = "Friis",
                Description = "skriver ibland på BraKod.se"
            });

            var actualJson = JsonConvert.SerializeObject(controller.GetFullConsultantProfiles().First());

            actualJson.ShouldBeEquivalentTo(expectedJson, "_ en registrerad och autentiserad användare ska kunna se all information i konsultprofilerna");

        }


        private static GenericPrincipal GetMockedCurrentUser(string name)
        {
            return new GenericPrincipal(new GenericIdentity(name), new[] { "RegistreradAnvändare" });
        }

        private ControllerContext GetMockedHttpContext_WithCurrentUser(ClaimsPrincipal currentUser)
        {
            //se svaren i denna tråd för mer info om hur du kan mocka IPrincipal och HttpContext i .net core
            //http://stackoverflow.com/questions/38557942/mocking-iprincipal-in-asp-net-core
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = currentUser
                }
            };
        }

        private IEnumerable<ConsultantProfile> GetMockedConsultantProfiles()
        {
            return new List<ConsultantProfile>
            {
                new ConsultantProfile
                {
                    FirstName = "Jimi", LastName = "Friis", Description = "skriver ibland på BraKod.se"
                }
            };
        }

    }
}
