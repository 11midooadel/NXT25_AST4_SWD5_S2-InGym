using NXT25_AST4_SWD5_S2_InGym.Data;

namespace NXT25_AST4_SWD5_S2_InGym.Services
{
    public class AccountService : IAccountService
    {
        private readonly GymDbContext _context;

        public AccountService(GymDbContext context)
        {
            _context = context;
        }
    }
}