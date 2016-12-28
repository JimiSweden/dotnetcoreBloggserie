using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using profil.api.Controllers;
using profil.api.Models;

namespace Profil.Api.Test
{

    public class ConsultantProfileControllerTest
    {
        [Fact]
        public void Unauthorized_user_can_get_profiles_names_from_api()
        {
            var repoMock = new Mock<IConsultantProfileRepository>();
            repoMock.Setup(x => x.GetAllLimited()).Returns(GetDefaultLimitedProfiles);

            var controller = new ConsultantProfileController(repoMock.Object);

            var expected = new ConsultantProfileLimitedViewModel
            {
                FirstName = "Jimi"
            };

            var actual = controller.GetLimitedConsultantProfiles().First();
			
			actual.GetType().Properties().Count().ShouldBeEquivalentTo(1, "_ Endast förnamnet får lämnas ut till icke registrerade användare");

			actual.ShouldBeEquivalentTo(expected, "För att det är så vi vill ha det");
        }

		
        private List<ConsultantProfileLimitedViewModel> GetDefaultLimitedProfiles()
        {
            return new List<ConsultantProfileLimitedViewModel>
            {
                new ConsultantProfileLimitedViewModel
                {
                    FirstName = "Jimi"
                }
            };
        }

    }
}
