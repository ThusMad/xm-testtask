using System.Threading.Tasks;

namespace XmPriceAgg.BLL.Interfaces;

public interface IAggregationService
{
    Task<float> RetrievePriceAsync(long timestamp);
    Task<IEnumerable<float>> RetrievePricesAsync(long timestampFrom, long timestampTo);
}