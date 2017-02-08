using System.Collections.Generic;

namespace Profil.ContentAdmin.ViewModels
{
    public class ConsultantProfilesViewModel
    {
        public IEnumerable<ConsultantProfileItemViewModel> ConsultantProfiles { get; set; }

        //detta gör att vi inte får "null reference exception" när vi loopar över profilerna i vyn
        public ConsultantProfilesViewModel()
        {
            ConsultantProfiles = new List<ConsultantProfileItemViewModel>();
        }
    }
}
