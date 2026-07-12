using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InGym.Data;
using InGym.Models;

namespace NXT25_AST4_SWD5_S2_InGym.Pages.Members
{
    public class CreateModel : PageModel
    {
        private readonly InGymContext _context;

        public CreateModel(InGymContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User.DateJoined = DateTime.Now;
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
