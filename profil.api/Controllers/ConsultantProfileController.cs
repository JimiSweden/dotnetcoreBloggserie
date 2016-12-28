using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using profil.api.Models;

namespace profil.api.Controllers
{
    [Route("api/[controller]")]
    public class ConsultantProfileController : Controller  
    {
        private readonly IConsultantProfileRepository consultantProfileRepository;

        public ConsultantProfileController(IConsultantProfileRepository consultantProfileRepository)
        {
            this.consultantProfileRepository = consultantProfileRepository;
        }

        [HttpGet]
        [Route("")]//default route, behöver anges eftersom jag vill ge metoden ett tydligare namn än "Get", 
        public IEnumerable<ConsultantProfileLimitedViewModel> GetLimitedConsultantProfiles()
        {
            return consultantProfileRepository.GetAllLimited();
        }

        [HttpGet]
        [Authorize]
        [Route("GetFull")]
        public IEnumerable<ConsultantProfile> GetFullConsultantProfiles()
        {
            return consultantProfileRepository.GetAll();
        }
    }
}
