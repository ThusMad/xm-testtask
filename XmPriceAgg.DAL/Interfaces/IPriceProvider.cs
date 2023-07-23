namespace XmPriceAgg.DAL.Interfaces;

public interface IPriceProvider
{
    public Task<float> GetPriceAsync(long timestamp, CancellationToken cancellationToken = default);

    public Task<IEnumerable<float>> GetPriceListAsync(long fromTimestamp, long toTimestamp, CancellationToken cancellationToken = default);
}