using System.Collections.Generic;
using System.Linq;

namespace profil.api.Models
{
    public interface IConsultantProfileRepository
    {
        IEnumerable<ConsultantProfile> GetAll();
        IEnumerable<ConsultantProfileLimitedViewModel> GetAllLimited();
    }

    //en första implementation utan att blanda in databaser
    public class InMemoryConsultantProfileRepository : IConsultantProfileRepository
    {
        private List<ConsultantProfile> consultantProfiles;

        public InMemoryConsultantProfileRepository()
        {
            consultantProfiles = new List<ConsultantProfile>
            {
                new ConsultantProfile { FirstName = "Jimi", LastName = "Friis", Description = "Systemutvecklare Fullstack - C# .net, javascript..." },
                new ConsultantProfile { FirstName = "Pontus", LastName = "Nagy", Description = "Systemutvecklare Frontend - javascript, React, Html..." },
                new ConsultantProfile { FirstName = "Mikael", LastName = "Sundström", Description = "Sysadmin - Windows, Mac..." },
            };
        }

        public IEnumerable<ConsultantProfile> GetAll()
        {
            return consultantProfiles;
        }

        public IEnumerable<ConsultantProfileLimitedViewModel> GetAllLimited()
        {
            return GetAll().Select(p => new ConsultantProfileLimitedViewModel { FirstName = p.FirstName });
        }
    }
}