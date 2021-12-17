using Bot.Entities;
using Dapper;
using IronPython.Infrastructure.Abstractions;
using IronPython.Infrastructure.Repository;
using IronPython.Infrastructure.DbContext.Extensions;
using Bot.Entities.Enums;

namespace Bot.Infrastructure.Repositories
{
    public class RecordRepository : BaseRepository<Record>
    {
        public RecordRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Record>> GetRecordsByType(RecordType type)
        {
            var connection = await DbContext.GetConnection();

            return (await connection.QueryAsync<Record>(@$"SELECT * FROM public.""{await DbContext.GetTableNameFromType<Record>()}"" " +
                $"WHERE \"{nameof(Record.Type)}\" = @Type;", new 
            {
                Type = (int)type
            })).ToList();
        }

        public async Task DeleteById(int id)
        {
            var connection = await DbContext.GetConnection();

            await connection.QueryAsync<Record>(@$"DELETE FROM public.""{await DbContext.GetTableNameFromType<Record>()}"" " +
                $"WHERE \"{nameof(Record.Id)}\" = @Id;", new
                {
                    Id = id
                });
        }
    }
}
