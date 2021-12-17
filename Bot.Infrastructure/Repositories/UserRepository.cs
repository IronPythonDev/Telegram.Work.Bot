using Bot.Entities;
using IronPython.Infrastructure.Abstractions;
using IronPython.Infrastructure.Repository;

namespace Bot.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
