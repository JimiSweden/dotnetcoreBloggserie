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
        [Authorize]
        public IEnumerable<ConsultantProfile> Get()
        {
            return consultantProfileRepository.GetAll();
        }
    }
}
