using Microsoft.AspNetCore.Mvc.RazorPages;
using EuroLink.Models;
using EuroLink.Services;

namespace EuroLink.Pages.Phones
{
    public class IndexModel : PageModel
    {
        private readonly IPhoneRepository _phoneRepository;

        public IndexModel(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }

        public List<Phone> Phones { get; set; } = new List<Phone>();

        public async Task OnGetAsync()
        {

            Phones = await _phoneRepository.GetAllPhonesAsync();
        }
    }
}