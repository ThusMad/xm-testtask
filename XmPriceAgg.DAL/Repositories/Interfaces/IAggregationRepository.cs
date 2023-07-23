using XmPriceAgg.DAL.Entities;

namespace XmPriceAgg.DAL.Repositories.Interfaces;

public interface IAggregationRepository
{
    public Task<StoredPriceEntity?> GetStoredPriceAsync(long timestamp);
    public Task<IEnumerable<StoredPriceEntity>> GetStoredPricesAsync(long timestampFrom, long timestampTo);
    public Task InsertStoredPriceAsync(long timestamp, float price);
}