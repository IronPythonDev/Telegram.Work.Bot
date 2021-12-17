using Bot;
using Bot.Infrastructure;
using Bot.Infrastructure.Managers;
using Bot.Infrastructure.Migrations;
using Bot.Infrastructure.Repositories;
using IronPython.Infrastructure.Abstractions;
using IronPython.Infrastructure.Migrations.Extensions;
using IronPython.Infrastructure.Npgsql;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

try
{
    var dbConnectionStringBuilder = new NpgsqlConnectionStringBuilder()
    {
        Host = "postgres",
        Port = 5432,
        Username = "postgres",
        Password = "1111",
        Database = "work_bot"
    };

    IDbContext DbContext = new DbContext(dbConnectionStringBuilder.ConnectionString);

    Statics.DbContext = DbContext;

    await MigrateDataBase(dbConnectionStringBuilder.Database);

    ActivatorExtensions.ServiceProvider = new ServiceCollection()
        .AddSingleton(DbContext)
        .AddSingleton<UserRepository>()
        .AddSingleton<RecordRepository>()
        .BuildServiceProvider();

    await CommandsManager.LoadHandlers(typeof(Program).Assembly);
    await CallbackQueryManager.LoadHandlers(typeof(Program).Assembly);

    var client = new TelegramBotClient(Configuration.Token);

    using var cts = new CancellationTokenSource();

    var receiverOptions = new ReceiverOptions()
    {
        AllowedUpdates = { }
    };

    client.StartReceiving<UpdateHandler>(receiverOptions, cts.Token);

    var me = await client.GetMeAsync();

    Console.WriteLine($"Start listening for @{me.Username}");

    while (true) await Task.Delay(Int32.MaxValue);

    async Task MigrateDataBase(string dbName)
    {
        Console.WriteLine("Started migrate database");

        var migrationAssembly = typeof(InitMigration).Assembly;

        var isExistDb = await DbContext.ExistsDatabase(dbName);

        if (isExistDb)
        {
            await DbContext.Migrate<NpgsqlConnection>(migrationAssembly);

            Console.WriteLine("Successfully migrated database");

            return;
        }

        await DbContext.CreateDataBase(dbName);

        await DbContext.Migrate<NpgsqlConnection>(migrationAssembly);

        Console.WriteLine("Successfully created and migrated database");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}