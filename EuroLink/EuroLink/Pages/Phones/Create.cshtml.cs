using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EuroLink.Models;
using EuroLink.Services;

namespace EuroLink.Pages.Phones
{
    public class CreateModel : PageModel
    {
        private readonly PhoneService _phoneService;

        public CreateModel(PhoneService phoneService)
        {
            _phoneService = phoneService;
        }

        [BindProperty]
        public Phone Phone { get; set; } = new Phone();

        public bool SuccessMessage { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _phoneService.AddPhoneToCatalogAsync(
                Phone.Brand,
                Phone.Model,
                Phone.Price,
                Phone.StockQuantity,
                Phone.Color,
                Phone.Memory,
                Phone.Description,
                Phone.ImageUrl);

            if (result.IsSuccess)
            {
                SuccessMessage = true;
                Phone = new Phone();
                ModelState.Clear();
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
            }

            return Page();
        }
    }
}