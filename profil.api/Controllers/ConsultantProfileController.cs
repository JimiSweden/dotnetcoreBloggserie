using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profil.Api.Models;

namespace Profil.Api.Controllers
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
        public IEnumerable<ConsultantProfileNameViewModel> GetAllNameOnly()
        {
            return consultantProfileRepository.GetAllNameOnly();
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
