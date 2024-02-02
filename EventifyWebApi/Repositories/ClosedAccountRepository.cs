using EventifyCommon.Context;
using EventifyCommon.Models;
using Microsoft.EntityFrameworkCore;

namespace EventifyWebApi.Repositories
{
    public class ClosedAccountRepository : IClosedAccount
    {
        private readonly ApplicationDbContext _db;

        public ClosedAccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ClosedAccount> Create(ClosedAccount closedAccount)
        {
            await _db.ClosedAccounts.AddAsync(closedAccount);
            await _db.SaveChangesAsync();
            return closedAccount;
        }

        public async Task<ClosedAccountReason?> GetClosedAccountReason(int id)
        {
            var closedAccountReason = await _db.ClosedAccountReasons.FirstOrDefaultAsync(c => c.Id == id);
            if (closedAccountReason == null)
            {
                return null;
            }

            return closedAccountReason;
        }

        public async Task<List<ClosedAccountReason>> GetClosedAccountReasons()
        {
            return await _db.ClosedAccountReasons.ToListAsync();
        }
    }
}
