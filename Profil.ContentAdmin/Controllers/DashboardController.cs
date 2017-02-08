using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Profil.ContentAdmin.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Profil.ContentAdmin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new ConsultantProfilesViewModel();
            model = GetDummyProfiles();
            return View(model);
        }

        private ConsultantProfilesViewModel GetDummyProfiles()
        {
            return new ConsultantProfilesViewModel
            {
                ConsultantProfiles = new List<ConsultantProfileItemViewModel>
                {
                    new ConsultantProfileItemViewModel
                    {
                        FirstName = "Jimi", LastName = "Friis",
                        Summary = "Gillar bra kod, affärsutvekling och golf",
                        Id = new Guid("00000000-0000-0000-0000-000000000001")
                    },
                    new ConsultantProfileItemViewModel
                    {
                        FirstName = "Pontus", LastName = "Nagy",
                        Summary = "Är vass på React och klär sig medvetet",
                        Id = new Guid("00000000-0000-0000-0000-000000000002")
                    },
                    new ConsultantProfileItemViewModel
                    {
                        FirstName = "Mikael", LastName = "Sundström",
                        Summary = "Teknik och bakning ligger honom i fatet",
                        Id = new Guid("00000000-0000-0000-0000-000000000003")
                    },
                }
            };
        }
    }
}
