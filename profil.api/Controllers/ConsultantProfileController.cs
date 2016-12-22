using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<ConsultantProfile> Get()
        {
            return consultantProfileRepository.GetAll();
        }
    }
}
