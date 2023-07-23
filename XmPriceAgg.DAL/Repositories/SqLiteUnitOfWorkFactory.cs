using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using XmPriceAgg.DAL.Repositories.Interfaces;

namespace XmPriceAgg.DAL.Repositories;

public class SqLiteUnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IConfiguration _configuration;

    public SqLiteUnitOfWorkFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IUnitOfWork Create()
    {
        var connectionString = _configuration["ConnectionStrings:SqLiteConnectionString"];
        var connection = new SqliteConnection(connectionString);

        return new UnitOfWork(connection);
    }
}