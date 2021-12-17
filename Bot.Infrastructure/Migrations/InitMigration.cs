using Bot.Entities;
using FluentMigrator;
using IronPython.Infrastructure.DbContext.Extensions;

namespace Bot.Infrastructure.Migrations
{
    [Migration(1)]
    public class InitMigration : Migration
    {
        public override async void Down()
        {
            Delete.Table(await Statics.DbContext.GetTableNameFromType(typeof(User)));
        }

        public override async void Up()
        {
            Create.Table(await Statics.DbContext.GetTableNameFromType(typeof(User)))
                .WithColumn(nameof(User.Id))
                    .AsInt32()
                    .Identity()
                    .PrimaryKey()
                .WithColumn(nameof(User.TelegramId))
                    .AsString()
                    .Nullable()
                    .WithDefaultValue(null)
                .WithColumn(nameof(User.InputState))
                    .AsString()
                    .Nullable()
                    .WithDefaultValue("")
                .WithColumn(nameof(User.LanguageCode))
                    .AsString()
                    .Nullable()
                    .WithDefaultValue(null)
                .WithColumn(nameof(User.TelegramUserName))
                    .AsString()
                    .Nullable()
                    .WithDefaultValue(null);
        }
    }
}
