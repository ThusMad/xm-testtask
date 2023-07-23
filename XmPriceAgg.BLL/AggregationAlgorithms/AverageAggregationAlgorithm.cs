using Microsoft.Extensions.Logging;
using XmPriceAgg.BLL.Interfaces;

namespace XmPriceAgg.BLL.AggregationAlgorithms;

public class AverageAggregationAlgorithm : IAggregationAlgorithm
{
    private readonly ILogger<AverageAggregationAlgorithm> _logger;

    public AverageAggregationAlgorithm(ILogger<AverageAggregationAlgorithm> logger)
    {
        _logger = logger;
    }

    public float Aggregate(params float[] prices)
    {
        throw new NotImplementedException();
    }

    public float Aggregate(IEnumerable<float> prices)
    {
        _logger.LogInformation("Aggregating prices using avg price method");

        return prices.Sum() / prices.Count();
    }
}