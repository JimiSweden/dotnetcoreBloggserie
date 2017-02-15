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


        public IActionResult Details(Guid id)
        {
            var model = new ConsultantProfileViewModel
            {
                FirstName = "Jimi",
                LastName = "Friis",
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Description = $"Affärsintresserad Prestigelös Agil Utvecklare med fokus på Rätt. Helikopterperspektiv – Har lätt för att överblicka och förstå hur processer, system och arbetsflöden hänger ihop.  Professionell – Tar ansvar och initiativ, frågar och vill veta varför innan vi gör.  Resultatinriktad – Drivs av att leverera bra lösningar på riktiga och viktiga problem, Rätt sak i Rätt tid ger Rätt kundnytta. Lagspelare – Trivs bäst i team och kan om det behövs ta ledande roller. ”Det optimala utvecklingsteamet enligt mig är tvärfunktionellt och agilt med mandat till förändringar och snabb tillgång till affärens områdesexperter” Som person är Jimi professionell, ödmjuk, serviceinriktad, social, ärlig, glad och lyhörd, men för den delen inte blyg i att göra sina åsikter hörda. Arbetsmetoder som TDD, BDD och DDD ligger Jimi varmt om hjärtat . Högskolestudier inom både företagsekonomi och datavetenskap, en IT-bakgrund inom serverdrift och nätverk samt erfarenhet från diverse olika yrkesområden utanför IT-världen och ett organisatoriskt sinne underlättar förståelsen för och anpassningen till olika typer av verksamheter."
            };
            return View(model);
        }


        public IActionResult Edit(Guid id)
        {
            var model = new ConsultantProfileViewModel
            {
                FirstName = "Jimi",
                LastName = "Friis",
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Description = $"Affärsintresserad Prestigelös Agil Utvecklare med fokus på Rätt. Helikopterperspektiv – Har lätt för att överblicka och förstå hur processer, system och arbetsflöden hänger ihop.  Professionell – Tar ansvar och initiativ, frågar och vill veta varför innan vi gör.  Resultatinriktad – Drivs av att leverera bra lösningar på riktiga och viktiga problem, Rätt sak i Rätt tid ger Rätt kundnytta. Lagspelare – Trivs bäst i team och kan om det behövs ta ledande roller. ”Det optimala utvecklingsteamet enligt mig är tvärfunktionellt och agilt med mandat till förändringar och snabb tillgång till affärens områdesexperter” Som person är Jimi professionell, ödmjuk, serviceinriktad, social, ärlig, glad och lyhörd, men för den delen inte blyg i att göra sina åsikter hörda. Arbetsmetoder som TDD, BDD och DDD ligger Jimi varmt om hjärtat . Högskolestudier inom både företagsekonomi och datavetenskap, en IT-bakgrund inom serverdrift och nätverk samt erfarenhet från diverse olika yrkesområden utanför IT-världen och ett organisatoriskt sinne underlättar förståelsen för och anpassningen till olika typer av verksamheter."
            };
            return View(model);
        }


        public IActionResult Create()
        {
            return View(new ConsultantProfileViewModel());
        }

        public IActionResult Delete(Guid id)
        {
            //en lyckad delete skickar användaren till listan
            return RedirectToAction("Index");
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
                        Summary = "Gillar bra kod, affärsutveckling och golf",
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
                        Summary = "Glänser inom teknik och bakning",
                        Id = new Guid("00000000-0000-0000-0000-000000000003")
                    },
                }
            };
        }
    }
}
