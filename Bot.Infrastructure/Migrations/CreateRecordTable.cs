using Bot.Entities;
using FluentMigrator;
using IronPython.Infrastructure.DbContext.Extensions;

namespace Bot.Infrastructure.Migrations
{
    [Migration(2)]
    public class CreateRecordTable : Migration
    {
        public override async void Down()
        {
            Delete.Table(await Statics.DbContext.GetTableNameFromType<Record>());
        }

        public override async void Up()
        {
            Create.Table(await Statics.DbContext.GetTableNameFromType<Record>())
                .WithColumn(nameof(Record.Id))
                    .AsInt32()
                    .PrimaryKey()
                    .Identity()
                .WithColumn(nameof(Record.Type))
                    .AsInt32()
                    .NotNullable()
                    .WithDefaultValue(-1)
                .WithColumn(nameof(Record.MultiplayerType))
                    .AsInt32()
                    .NotNullable()
                    .WithDefaultValue(0)
                .WithColumn(nameof(Record.Header))
                    .AsString()
                    .Nullable()
                    .WithDefaultValue("")
                .WithColumn(nameof(Record.Requirements))
                    .AsString()
                    .Nullable()
                    .WithDefaultValue("")
                .WithColumn(nameof(Record.Details))
                    .AsString()
                    .Nullable()
                    .WithDefaultValue("")
                .WithColumn(nameof(Record.OwnerId))
                    .AsInt32()
                    .ForeignKey(await Statics.DbContext.GetTableNameFromType<User>(), nameof(User.Id))
                    .OnDelete(System.Data.Rule.Cascade);
        }
    }
}
