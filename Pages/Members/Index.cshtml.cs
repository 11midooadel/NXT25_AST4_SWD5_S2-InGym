using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using InGym.Data;
using InGym.Models;

namespace NXT25_AST4_SWD5_S2_InGym.Pages.Members
{
    public class IndexModel : PageModel
    {
        private readonly InGymContext _context;

        public IndexModel(InGymContext context)
        {
            _context = context;
        }

        public IList<User> Users { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }
    }
}
