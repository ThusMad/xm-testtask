using System.Data;
using Dapper;
using XmPriceAgg.DAL.Entities;
using XmPriceAgg.DAL.Repositories.Interfaces;

namespace XmPriceAgg.DAL.Repositories;

public class AggregationRepository : RepositoryBase, IAggregationRepository
{
    private const string CreateTableSql =
        @"
        CREATE TABLE IF NOT EXISTS StoredPrices (
            Timestamp INTEGER NOT NULL PRIMARY KEY UNIQUE,
            Price REAL NOT NULL
        );";

    public AggregationRepository(IDbTransaction? transaction) : base(transaction)
    {
        Connection?.Execute(CreateTableSql);
    }

    public async Task<StoredPriceEntity?> GetStoredPriceAsync(long timestamp)
    {
        const string sql = "SELECT * FROM StoredPrices WHERE Timestamp = @Timestamp";

        var parameters = new
        {
            Timestamp = timestamp
        };

        return await Connection.QueryFirstOrDefaultAsync<StoredPriceEntity>(sql, parameters);
    }


    public async Task<IEnumerable<StoredPriceEntity>> GetStoredPricesAsync(long timestampFrom, long timestampTo)
    {
        const string sql = "SELECT * FROM StoredPrices WHERE Timestamp >= @TimestampFrom AND Timestamp <= @TimestampTo";

        var parameters = new
        {
            TimestampFrom = timestampFrom,
            TimestampTo = timestampTo
        };

        return await Connection.QueryAsync<StoredPriceEntity>(sql, parameters);
    }

    public async Task InsertStoredPriceAsync(long timestamp, float price)
    {
        const string sql = "INSERT INTO StoredPrices (Timestamp, Price) VALUES (@Timestamp, @Price)";

        var parameters = new
        {
            Timestamp = timestamp,
            Price = price
        };

        await Connection.ExecuteAsync(sql, parameters, Transaction);
    }
}