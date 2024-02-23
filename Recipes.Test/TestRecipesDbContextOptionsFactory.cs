using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Recipes.Core.Infrastructure.Database;

namespace Recipes.Test;

public class TestRecipesDbContextOptionsFactory : RecipesDbContextOptionsFactory, IDisposable
{
    private readonly SqliteConnection _sqliteConnection;
    
    public TestRecipesDbContextOptionsFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _sqliteConnection = new SqliteConnection("Filename=:memory:");
        _sqliteConnection.Open();
    }

    protected override void ConfigureDatabaseProvider(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite(_sqliteConnection);
    }

    public void Dispose()
    {
        _sqliteConnection.Close();
        _sqliteConnection.Dispose();
    }
}