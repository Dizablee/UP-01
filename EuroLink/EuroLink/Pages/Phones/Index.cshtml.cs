using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EuroLink.Models;
using EuroLink.Services;

namespace EuroLink.Pages.Phones
{
    public class IndexModel(IPhoneRepository phoneRepository, PhoneService phoneService) : PageModel
    {
        private readonly IPhoneRepository _phoneRepository = phoneRepository;
        private readonly PhoneService _phoneService = phoneService;

        public List<Phone> Phones { get; set; } = new();

        [BindProperty]
        public List<int> SelectedPhoneIds { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public bool ShowDeletePanel { get; set; }

        public void OnGet()
        {
            Phones = _phoneRepository.GetAllPhones();
        }

        public IActionResult OnPostToggleDeleteMode()
        {
            ShowDeletePanel = true;
            Phones = _phoneRepository.GetAllPhones();
            return Page();
        }

        public IActionResult OnPostCancelDelete()
        {
            ShowDeletePanel = false;
            SelectedPhoneIds.Clear();
            Phones = _phoneRepository.GetAllPhones();
            return Page();
        }

        public IActionResult OnPostDeletePhones()
        {
            if (!SelectedPhoneIds.Any())
            {
                ErrorMessage = "Не выбрано ни одного телефона для удаления";
                ShowDeletePanel = true;
                Phones = _phoneRepository.GetAllPhones();
                return Page();
            }

            var result = _phoneService.DeletePhonesFromCatalog(SelectedPhoneIds);

            if (result == null)
            {
                // Успешное удаление
                return RedirectToPage();
            }
            else
            {
                // Ошибка удаления
                ErrorMessage = result;
                ShowDeletePanel = true;
                Phones = _phoneRepository.GetAllPhones();
                return Page();
            }
        }
    }
}