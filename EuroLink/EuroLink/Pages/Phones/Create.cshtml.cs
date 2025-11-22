using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EuroLink.Models;
using EuroLink.Services;

namespace EuroLink.Pages.Phones
{
    public class CreateModel(PhoneService phoneService) : PageModel
    {
        private readonly PhoneService _phoneService = phoneService;

        [BindProperty]
        public Phone Phone { get; set; } = new();

        public bool SuccessMessage { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = _phoneService.AddPhoneToCatalog(
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
                Phone = new();
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