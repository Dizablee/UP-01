using EuroLink.Models;
using EuroLink.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EuroLink.Pages.Phones
{
    public class IndexModel(IPhoneRepository phoneRepository) : PageModel
    {
        private readonly IPhoneRepository _phoneRepository = phoneRepository;

        public List<Phone> Phones { get; set; } = [];

        public void OnGet()
        {
            Phones = _phoneRepository.GetAllPhones();
        }
    }
}