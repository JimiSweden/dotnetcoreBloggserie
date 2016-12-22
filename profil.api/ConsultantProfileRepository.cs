using System.Collections.Generic;
using System.Linq;

namespace profil.api
{
    public interface IConsultantProfileRepository
    {
        IEnumerable<ConsultantProfile> GetAll();
        IEnumerable<ConsultantProfileLimitedViewModel> GetAllLimited();
    }

    public class ConsultantProfileLimitedViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class ConsultantProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
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
            return consultantProfiles.Select(p => new ConsultantProfileLimitedViewModel { FirstName = p.FirstName, LastName = p.LastName });
        }
    }
}